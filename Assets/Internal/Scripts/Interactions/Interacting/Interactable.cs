using System;
using Fusion;
using Shakhtarsk.Characters.Player;

namespace Shakhtarsk.Interactions
{
    public abstract class Interactable : NetworkBehaviour, IInteractable
    {
        public event Action<bool> FocusChanged = delegate {};

        public NetworkBehaviourId BehaviourId => Id;

        public void Focus()
        {
            OnFocus();
            FocusChanged.Invoke(true);
        }

        public void UnFocus()
        {
            OnUnFocus();
            FocusChanged.Invoke(false);
        }

        public void Interact(PlayerResolver playerResolver)
        {
            OnInteract(playerResolver);
        }
        
        public void Release(PlayerResolver playerResolver)
        {
            OnRelease(playerResolver);
        }

        protected virtual void OnFocus() { }
        protected virtual void OnUnFocus() { }
        protected virtual void OnInteract(PlayerResolver playerResolver) { }
        protected virtual void OnRelease(PlayerResolver playerResolver) { }
    }
}