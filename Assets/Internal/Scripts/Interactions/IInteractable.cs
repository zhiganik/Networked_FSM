using Fusion;
using Shakhtarsk.Characters.Player;

namespace Shakhtarsk.Interactions
{
    public interface IInteractable
    {
        NetworkBehaviourId BehaviourId { get; }
        
        void Focus();
        void UnFocus();
        void Interact(PlayerResolver playerResolver);
        void Release(PlayerResolver playerResolver);
    }
}