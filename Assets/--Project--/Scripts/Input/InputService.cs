using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace __Project__.Scripts.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly PlayerInput _playerInput;

        public event Action OnJump;
        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnLook;

        private const string MoveAction = "Move";

        public InputService()
        {
            _playerInput = new PlayerInput();
            
        }
        
        public void Initialize()
        {
            _playerInput.Player.Move.performed += Move;
            _playerInput.Player.Move.canceled += Move;
            
            _playerInput.Player.Look.performed += Look;
            _playerInput.Player.Look.canceled += Look;
            
            _playerInput.Player.Jump.performed += Jump;
            
            _playerInput.Enable();
        }

        public void Dispose()
        {
            _playerInput.Player.Move.performed -= Move;
            _playerInput.Player.Move.canceled -= Move;
            
            _playerInput.Player.Look.performed -= Look;
            _playerInput.Player.Look.canceled -= Look;
            
            _playerInput.Player.Jump.performed -= Jump;
            
            _playerInput.Disable();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void Look(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            OnLook?.Invoke(value);
        }

        private void Move(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            OnMove?.Invoke(value);
        }
    }
}
