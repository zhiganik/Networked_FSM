using Fusion.Addons.KCC;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.Features.Processors
{
    public sealed class SprintProcessor : KCCProcessor, ISetKinematicSpeed
    {
        [SerializeField] private float kinematicSpeedMultiplier = 2.0f;
        [SerializeField] private float staminaDrain = 10f;
        
        public override float GetPriority(KCC kcc) => kinematicSpeedMultiplier;

        public void Execute(ISetKinematicSpeed stage, KCC kcc, KCCData data)
        {
            if (data.Sprint)
            {
                data.KinematicSpeed *= kinematicSpeedMultiplier;
                // Suppress other sprint processors with lower priority.
                kcc.SuppressProcessors<SprintProcessor>();

                // Following call can be used to suppress other processors with lower priority implementing IAbilityProcessor (simulating a category identified by the interface).
                //kcc.SuppressProcessors<IAbilityProcessor>();

                // Following call can be used to suppress other ISetKinematicSpeed processors with lower priority.
                //kcc.SuppressProcessors<ISetKinematicSpeed>();
            }
        }
    }
}