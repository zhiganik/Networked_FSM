using R3;
using UnityEngine;

namespace Shakhtarsk.Input
{
    public interface IInputService
    {
        ReadOnlyReactiveProperty<bool> IsCrouch { get; }
        ReadOnlyReactiveProperty<bool> IsSprint { get; }
        ReadOnlyReactiveProperty<bool> IsJump { get; }
        ReadOnlyReactiveProperty<bool> IsInteract { get; }
        ReadOnlyReactiveProperty<bool> IsRelease { get; }
        ReadOnlyReactiveProperty<Vector2> Look { get; }
        ReadOnlyReactiveProperty<Vector2> Move { get; }


        void ActivateInput(bool state);
    }
}