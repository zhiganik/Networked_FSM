using System;
using __Project__.Scripts.Input;
using __Project__.Scripts.Player;
using __Project__.Scripts.Sensors;
using __Project__.Scripts.Tools;
using Fusion;
using UnityEngine;

namespace __Project__.Scripts.Network
{
    [RequireComponent(typeof(NetworkObject), typeof(CharacterController))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [Header("References"), Space]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private AnimationSystem animationsSystem;
        [SerializeField] private Animator animator;
        [SerializeField] private SkinnedMeshRenderer[] meshesToDisable;
        
        [Header("Bones"), Space]
        [SerializeField] private Transform leftEye;
        [SerializeField] private Transform rightEye;
        [SerializeField] private Transform headBone;

        [Header("Settings"), Space] 
        [SerializeField] private float smoothFactor = 0.02f;
        [SerializeField] private float cameraSensitive = 5f;
        [SerializeField] private float cameraHeight;
        [SerializeField] private float jumpForce;
        [SerializeField] private float maxVerticalAngle = 80f;
        [SerializeField] private float jumpTimer;
        
        private float _xRotation = 0f;
        private float _yRotation = 0f;
        private float _yForce = 0f;

        private Camera _camera;
        private float _currentSpeed;
        private bool _isMoving;
        private ActionTimer _jump;
        private RaySensor _floorRaySensor;
        private GameObject _head;
        private Vector2 _currentInput;
        private Vector2 _targetInput;
        private Vector2 _targetMouse;
        private IInputService _inputService;

        public bool IsMoving => _isMoving;
        public bool IsSprint { get; private set; }
        public bool IsCrouch { get; private set; }
        public bool IsJumping => _jump.IsRunning;
        public bool IsGrounded => _floorRaySensor.HasDetectedHit() && _floorRaySensor.GetDistance() < 0.1f;
        
        public Vector2 CurrentInput => _currentInput;
        public CharacterController CharacterController => characterController;

        public override void Spawned()
        {
            if(!HasStateAuthority) return;
            
            _head = new GameObject("Head");
            var headTransform = _head.transform;
            headTransform.SetParent(headBone);
            
            _camera = Camera.main;
            
            var camPos = (leftEye.localPosition + rightEye.localPosition) * 0.5f;
            headTransform.localPosition = camPos;
            headTransform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (var meshRenderer in meshesToDisable)
            {
                meshRenderer.enabled = false;
            }
            
            _floorRaySensor = new RaySensor(transform, characterController.radius);
            _floorRaySensor.SetCastDirection(CastDirection.Down);
            _floorRaySensor.SetLayerMask(LayerMask.NameToLayer("Floor"));

            _jump = new ActionTimer(jumpTimer);
        }
        
        public void Initialize(IInputService inputService)
        {
            _inputService = inputService;
            
            _inputService.OnMove += UpdateMoveDirection;
            _inputService.OnLook += UpdateLookDirection;
            _inputService.OnJump += Jump;
            _inputService.OnSprint += Sprint;
            _inputService.OnCrouch += Crunch;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if(!HasStateAuthority) return;
            
            _inputService.OnMove -= UpdateMoveDirection;
            _inputService.OnLook -= UpdateLookDirection;
            _inputService.OnJump -= Jump;
            _inputService.OnSprint -= Sprint;
            _inputService.OnCrouch -= Crunch;
        }

        private void Sprint(bool state)
        {
            if(IsCrouch) return;
            
            IsSprint = state;
        }
        
        private void Crunch()
        {
            if(!IsGrounded) return;
            
            IsCrouch = !IsCrouch;
        }

        private void Jump()
        {
            if(!IsGrounded || IsCrouch) return;

            _yForce = jumpForce;
            _jump.Launch();
        }

        private void UpdateMoveDirection(Vector2 moveDirection)
        {
            _targetInput = moveDirection;
            _isMoving = _targetInput != Vector2.zero;
        }

        private void UpdateLookDirection(Vector2 value)
        {
            _targetMouse = value;
        }

        private void Update()
        {
            HandleFloor();
            HandleGravity();
            HandleMovement(_currentSpeed);
            HandleRotation(cameraSensitive);
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var castPos = transform.position + (characterController.center - Vector3.up * (characterController.height / 2));
            Gizmos.DrawSphere(castPos, 0.02f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_floorRaySensor.GetPosition(), 0.02f);
            Gizmos.DrawLine(castPos, _floorRaySensor.GetPosition());
            
            Debug.LogError(IsGrounded);
        }

        public void HandleFloor()
        {
            var castPos = transform.position + (characterController.center - Vector3.up * (characterController.height / 2));
            _floorRaySensor.SetCastOrigin(castPos);
            _floorRaySensor.Cast();
        }

        public void HandleGravity()
        {
            _yForce += Physics.gravity.y * Time.deltaTime;

            if (!IsJumping && IsGrounded)
            {
                _yForce = -0.5f;
            }
        }

        public void HandleMovement(float speed)
        {
            var movementDirection = new Vector3(_currentInput.x, 0, _currentInput.y);
            var input = Mathf.Clamp01(movementDirection.magnitude);
            var move = input * speed;

            var cameraTransform = _camera.transform;
            var cameraDirection = cameraTransform.forward;
            cameraDirection.y = 0;
            var targetRotation = Quaternion.LookRotation(cameraDirection.normalized, Vector3.up);
            
            movementDirection = targetRotation * movementDirection;
            
            Vector2 currentVelocity = Vector2.zero; 
            _currentInput = Vector2.SmoothDamp(_currentInput, _targetInput, ref currentVelocity, smoothFactor);
            
            var velocity = movementDirection.normalized * move;
            velocity.y = _yForce;
            characterController.Move(velocity * Time.deltaTime);
        }

        public void HandleRotation(float speed)
        {
            var cameraTransform = _camera.transform;
            float mouseX = _targetMouse.x * speed * Time.deltaTime;
            float mouseY = _targetMouse.y * speed * Time.deltaTime;

            _xRotation -= mouseY;
            _yRotation += mouseX;
            _xRotation = Mathf.Clamp(_xRotation, -maxVerticalAngle, maxVerticalAngle);

            cameraTransform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
            
            var cameraDirection = cameraTransform.forward;
            cameraDirection.y = 0;
            var targetRotation = Quaternion.LookRotation(cameraDirection.normalized, Vector3.up);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        private void LateUpdate()
        {
            _camera.transform.position = _head.transform.position;
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtWeight(1f,0.4f,0.8f);
            animator.SetLookAtPosition(_camera.transform.forward);
        }
    }
}