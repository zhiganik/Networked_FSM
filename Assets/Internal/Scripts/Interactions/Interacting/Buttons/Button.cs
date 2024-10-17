using System;
using Fusion;
using Shakhtarsk.Characters.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Shakhtarsk.Interactions
{
    public class Button : Interactable
    {
        [SerializeField] private UnityEvent clicked;
        
        public event UnityAction Clicked
        {
            add => clicked.AddListener(value);
            remove => clicked.RemoveListener(value);
        }
        
        internal event Action LocalClicked;

        protected override void OnInteract(PlayerResolver playerResolver)
        {
            base.OnInteract(playerResolver);
            
            RPC_Interact();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_Interact()
        {
            LocalClicked?.Invoke();

            if (Object.HasStateAuthority)
            {
                clicked?.Invoke();

            }
        }
    }
}