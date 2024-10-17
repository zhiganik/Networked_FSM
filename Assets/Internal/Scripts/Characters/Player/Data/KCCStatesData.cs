using UnityEngine;

namespace Shakhtarsk.Characters.Player.Data
{
    [CreateAssetMenu(fileName = "KccData", menuName = "shakhtarsk/KccData", order = 0)]
    public class KCCStatesData : ScriptableObject
    {
        [SerializeField] private float defaultHeight = 1.79f;
        [SerializeField] private float crouchHeight = 1.3f;
        [SerializeField] private float jumpHeight = 1f;

        public float DefaultHeight => defaultHeight;
        public float CrouchHeight => crouchHeight;
        public float JumpHeight => jumpHeight;
    }
}

