using __Project__.Scripts.Input;
using Fusion;
using UnityEngine;

namespace __Project__.Scripts.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float moveSpeed;
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

            var velocity = new Vector3(_currentInput.x, 0, _currentInput.y) * moveSpeed;
            characterController.Move(velocity);
        }
    }
}