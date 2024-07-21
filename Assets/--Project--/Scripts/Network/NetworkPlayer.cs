using System;
using __Project__.Scripts.Input;
using __Project__.Scripts.Player;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace __Project__.Scripts.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [FormerlySerializedAs("animationsSystem")] [SerializeField] private AnimationsSystemODD animationsSystemOdd;
        [SerializeField] private Animator animator;
        
        
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float cameraHeight;
        
        private Vector2 _currentInput;
        private IInputService _inputService;
        
        public override void Spawned()
        {
            if(!HasStateAuthority) return;

            var camera = Camera.main;
            var cameraTransform = camera.transform;
            cameraTransform.parent = transform;
            cameraTransform.localPosition = Vector3.up * cameraHeight;
            cameraTransform.localRotation = Quaternion.identity;
        }

        public void Initialize(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnMovePerformed += UpdateVelocity;
            _inputService.OnMoveCanceled += CancelMove;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if(!HasStateAuthority) return;
            
            _inputService.OnMovePerformed -= UpdateVelocity;
            _inputService.OnMoveCanceled -= CancelMove;
        }

        private void UpdateVelocity(Vector2 value)
        {
            _currentInput = value;
        }

        private void CancelMove()
        {
            _currentInput = Vector2.zero;
        }

        private void Update()
        {
            if(!HasStateAuthority) return;

            var movementDirection = new Vector3(_currentInput.x, 0, _currentInput.y);
            var input = Mathf.Clamp01(movementDirection.magnitude);
            var speed = input * moveSpeed;
            movementDirection = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
            movementDirection.Normalize();

            var velocity = movementDirection * speed;
            animationsSystemOdd.UpdateLocomotion(velocity, moveSpeed);

            if (movementDirection != Vector3.zero)
            {
                var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        public void OnAnimatorMove()
        {
            var velocity = animator.deltaPosition;
            Debug.Log($"ANimator delta is {velocity}");
            characterController.Move(velocity);
        }
    }
}