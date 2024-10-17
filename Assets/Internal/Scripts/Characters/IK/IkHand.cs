using RootMotion.FinalIK;
using UnityEngine;

namespace Shakhtarsk.Characters.IK
{
    [ExecuteAlways]
    public class IkHand : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer handMesh;
        [SerializeField] private InteractionTarget interactionTarget;
        [SerializeField] private HandType handType;

        public HandType HandType => handType;

        public SkinnedMeshRenderer HandMesh => handMesh;
        public InteractionTarget InteractionTarget => interactionTarget;

        public void SetHandsActive(bool state)
        {
            handMesh.enabled = state;
        }
    }
}