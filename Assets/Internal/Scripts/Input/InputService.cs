using System;
using R3;
using UnityEngine;

namespace Shakhtarsk.Input
{
    public class InputService : IInputService, IDisposable
    {
        private readonly PlayerInput _playerInput;
        
        private readonly InputActionObservable<Vector2> _move;
        private readonly InputActionObservable<Vector2> _look;
        private readonly InputActionObservable<bool> _crouch;
        private readonly InputActionObservable<bool> _sprint;
        private readonly InputActionObservable<bool> _release;
        private readonly InputActionObservable<bool> _jump;
        private readonly InputActionObservable<bool> _interact;
        
        private CompositeDisposable _compositeDisposable;

        public ReadOnlyReactiveProperty<bool> IsCrouch => _crouch;
        public ReadOnlyReactiveProperty<bool> IsSprint => _sprint;
        public ReadOnlyReactiveProperty<bool> IsJump => _jump;
        public ReadOnlyReactiveProperty<bool> IsInteract => _interact;
        public ReadOnlyReactiveProperty<bool> IsRelease => _release;
        public ReadOnlyReactiveProperty<Vector2> Look => _look;
        public ReadOnlyReactiveProperty<Vector2> Move => _move;

        public InputService()
        {
            _playerInput = new PlayerInput();
            _move = new VectorInputAction<Vector2>(_playerInput.Player.Move, InputActionEventType.Default);
            _look = new VectorInputAction<Vector2>(_playerInput.Player.Look,  InputActionEventType.Default);
            _sprint = new ButtonInputAction(_playerInput.Player.Sprint,  InputActionEventType.Default);
            _crouch = new ButtonInputAction(_playerInput.Player.Crouch,  InputActionEventType.Default);
            _jump = new ButtonInputAction(_playerInput.Player.Jump,  InputActionEventType.Default);
            _interact = new ButtonInputAction(_playerInput.Player.Interact,  InputActionEventType.Default);
            _release = new ButtonInputAction(_playerInput.Player.Release,  InputActionEventType.Default);
        }

        public void Dispose()
        {
            ActivateInput(false);
        }
        
        public void ActivateInput(bool state)
        {
            if (state)
            {
                _playerInput.Enable();
            }
            else
            {
                _playerInput.Disable();
            }
        }
    }
}
