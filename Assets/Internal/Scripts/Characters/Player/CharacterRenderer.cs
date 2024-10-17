using Fusion;
using UnityEngine;

namespace Shakhtarsk.Characters.Player
{
    [RequireComponent(typeof(Animator))]
    public class CharacterRenderer : NetworkBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer[] meshesToDisable;
        
        public override void Spawned()
        {
            if(Object.IsProxy) return;
            
            DisableMeshes();
        }

        public override void Render()
        {
            DisableMeshes();
        }

        private void DisableMeshes()
        {
            if(Object.IsProxy) return;

            foreach (var meshRenderer in meshesToDisable)
            {
                meshRenderer.enabled = false;
            }
        }
    }
}