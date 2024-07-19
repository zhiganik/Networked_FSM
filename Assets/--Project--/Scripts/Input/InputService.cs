using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace __Project__.Scripts.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly PlayerInput _playerInput;
        
        private InputAction _move;

        public event Action<Vector2> OnMovePerformed;

        private const string MoveAction = "Move";

        public InputService(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            InitActions();
        }

        private void InitActions()
        {
            _move = _playerInput.actions[MoveAction];
        }

        public void Initialize()
        {
            _move.performed += OnMove;
        }
        
        public void Dispose()
        {
            _move.performed -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            OnMovePerformed?.Invoke(value);
        }
    }
}
