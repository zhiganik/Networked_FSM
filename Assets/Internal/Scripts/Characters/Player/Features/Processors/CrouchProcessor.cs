using Fusion.Addons.KCC;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.Features.Processors
{
    public sealed class CrouchProcessor : KCCProcessor, ISetKinematicSpeed
    {
        [SerializeField] private float kinematicSpeedMultiplier = 0.5f;
        
        public override float GetPriority(KCC kcc) => kinematicSpeedMultiplier;
        

        public void Execute(ISetKinematicSpeed stage, KCC kcc, KCCData data)
        {
            if (data.Crouch)
            {
                data.KinematicSpeed *= kinematicSpeedMultiplier;
                kcc.SuppressProcessors<CrouchProcessor>();
            }
        }
    }
}