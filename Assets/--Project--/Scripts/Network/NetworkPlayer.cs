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
        [SerializeField] private SkinnedMeshRenderer[] meshesToDisable; // for FP only
        
        [Header("Bones"), Space]
        [SerializeField] private Transform leftEye;
        [SerializeField] private Transform rightEye;
        [SerializeField] private Transform headBone;
        
        [Header("Settings"), Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float cameraHeight;
        [SerializeField] private float jumpForce;
        [SerializeField] private float maxVerticalAngle = 80f; // To limit vertical rotation
        
        private ActionTimer _jumpTimer;
        
        private float _xRotation = 0f; // Rotation around the X-axis
        private float _yRotation = 0f; // Rotation around the Y-axis
        private float _yForce = 0f;

        private bool _isMove;
        private RaySensor _floorRaySensor;
        private GameObject _head;
        private Vector3 _currentVelocity;
        private Vector2 _currentInput;
        private IInputService _inputService;
        
        public bool IsMove => _isMove;
        public bool IsJump => _jumpTimer.IsRunning;
        public bool IsGrounded => _floorRaySensor.HasDetectedHit();

        public Vector2 CurrentInput => _currentInput;

        public override void Spawned()
        {
            if(!HasStateAuthority) return;
            
            _head = new GameObject("Head");
            var headTransform = _head.transform;
            headTransform.SetParent(headBone);
            
            var camPos = (leftEye.localPosition + rightEye.localPosition) * 0.5f;
            headTransform.localPosition = camPos;
            headTransform.localRotation = Quaternion.identity;

            Cursor.lockState = CursorLockMode.Locked;

            foreach (var meshRenderer in meshesToDisable)
            {
                meshRenderer.enabled = false;
            }

            _floorRaySensor = new RaySensor(transform);
            _floorRaySensor.SetCastDirection(CastDirection.Down);
            _floorRaySensor.SetCastLength(0.1f);
            _floorRaySensor.SetLayerMask(LayerMask.NameToLayer("Floor"));

            _jumpTimer = new ActionTimer(0.2f);
        }

        public void Initialize(IInputService inputService)
        {
            _inputService = inputService;
            
            _inputService.OnMove += UpdateMoveDirection;
            _inputService.OnLook += UpdateLookDirection;
            _inputService.OnJump += Jump;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if(!HasStateAuthority) return;
            
            _inputService.OnMove -= UpdateMoveDirection;
            _inputService.OnLook -= UpdateLookDirection;
            _inputService.OnJump -= Jump;
        }

        private void Jump()
        {
            if(!IsGrounded) return;

            _jumpTimer.Launch();
            _yForce = jumpForce;
        }

        private void UpdateLookDirection(Vector2 value)
        {
            // Calculate the mouse delta movement
            float mouseX = value.x * rotationSpeed * Time.deltaTime;
            float mouseY = value.y * rotationSpeed * Time.deltaTime;

            _xRotation -= mouseY;
            _yRotation += mouseX;
            _xRotation = Mathf.Clamp(_xRotation, -maxVerticalAngle, maxVerticalAngle); // Clamp to avoid over-rotation

            Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 5f);
            
            Gizmos.color = Color.red;
            
            if(_floorRaySensor == null) return;
            
            Gizmos.DrawSphere(transform.TransformPoint(_floorRaySensor.OriginPoint), 0.02f);
            
            if(IsGrounded)
                Gizmos.DrawLine(transform.TransformPoint(_floorRaySensor.OriginPoint), _floorRaySensor.GetPosition());
        }

        private void UpdateMoveDirection(Vector2 moveDirection)
        {
            _currentInput = moveDirection;
            _isMove = _currentInput != Vector2.zero;
        }

        private void Update()
        {
            if(!HasStateAuthority) return;
            
            _floorRaySensor.SetCastOrigin(transform.position);
            _floorRaySensor.Cast();
            
            _yForce += Physics.gravity.y * Time.deltaTime;

            if (_yForce < 0)
            {
                _yForce = -0.5f;
            }
            
            var movementDirection = new Vector3(_currentInput.x, 0, _currentInput.y);
            var input = Mathf.Clamp01(movementDirection.magnitude);
            var move = input * moveSpeed;
            movementDirection = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
            movementDirection.Normalize();

            var velocity = movementDirection * move;
            velocity.y = _yForce;
            characterController.Move(velocity * Time.deltaTime);
            
            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5f);
            }
        }

        private void LateUpdate()
        {
            Camera.main.transform.position = _head.transform.position;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtWeight(1,0.8f,1f);
            animator.SetLookAtPosition(Camera.main.transform.forward * 5f);
        }
    }
}