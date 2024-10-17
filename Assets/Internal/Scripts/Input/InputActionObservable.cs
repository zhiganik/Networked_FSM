using R3;
using UnityEngine.InputSystem;

namespace Shakhtarsk.Input
{
    public class VectorInputAction<T> : InputActionObservable<T> where T : struct
    {
        public VectorInputAction(InputAction inputAction, InputActionEventType type = InputActionEventType.All) : base(inputAction, type)
        {
        }

        protected override void Invoke(InputAction.CallbackContext action)
        {
            Value = action.ReadValue<T>();
        }
    }

    public class ButtonInputAction : InputActionObservable<bool>
    {
        public ButtonInputAction(InputAction inputAction, InputActionEventType type = InputActionEventType.All) : base(inputAction, type)
        {
            
        }

        protected override void Invoke(InputAction.CallbackContext action)
        {
            Value = action.ReadValueAsButton();
        }
    }

    public abstract class InputActionObservable<T> : ReactiveProperty<T> where T : struct
    {
        private readonly InputAction _inputAction;

        public InputActionObservable(InputAction inputAction, InputActionEventType type = InputActionEventType.All)
        {
            _inputAction = inputAction;

            if (type.HasFlag(InputActionEventType.Started))
                _inputAction.started += Invoke;
            
            if (type.HasFlag(InputActionEventType.Performed))
                _inputAction.performed += Invoke;

            if (type.HasFlag(InputActionEventType.Canceled))
                _inputAction.canceled += Invoke;
        }

        protected abstract void Invoke(InputAction.CallbackContext action);
    }
}