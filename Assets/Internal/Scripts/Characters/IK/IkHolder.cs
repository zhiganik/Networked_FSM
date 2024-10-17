using System;
using RootMotion.FinalIK;
using Shakhtarsk.Characters.Player;
using Shakhtarsk.Interactions;
using UnityEngine;

namespace Shakhtarsk.Characters.IK
{
    public class IkHolder : MonoBehaviour, IPlayerComponent
    {
        [SerializeField] private Transform twoHandedPoint;
        [SerializeField] private Transform oneHandedPoint;

        [SerializeField] private FullBodyBipedIK ik;
        [SerializeField] private InteractionSystem ikInteraction;
        
        public FullBodyBipedIK IK => ik;

        public InteractionSystem IKInteraction => ikInteraction;

        public Transform GetPointByAttachType(AttachType attachType)
        {
            return attachType switch
            {
                AttachType.OneHanded => oneHandedPoint,
                AttachType.TwoHanded => twoHandedPoint,
                _ => throw new ArgumentOutOfRangeException(nameof(attachType), attachType, null)
            };
        }
    }
}