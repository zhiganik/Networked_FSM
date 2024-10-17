using Shakhtarsk.Characters.Player;
using UnityEngine;

namespace Shakhtarsk.Interactions
{
    public class DebugInteractable : Interactable
    {
        protected override void OnInteract(PlayerResolver playerResolver)
        {
            Debug.Log("Interact with Debug Interactable");
        }
    }
}