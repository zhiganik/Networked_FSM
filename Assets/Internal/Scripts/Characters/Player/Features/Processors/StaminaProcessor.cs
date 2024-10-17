using Fusion.Addons.KCC;
using R3;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.Features.Processors
{
    public class StaminaProcessor : KCCProcessor, IAfterMoveStep
    {
        [SerializeField] private Vector2 bounds = new Vector2(0, 100f);
        [SerializeField] private float sprintDrain = 2f;
        [SerializeField] private float jumpDrain = 20f;
        [SerializeField] private float staminaGain = 2f;
        [SerializeField] private float timeToGain = 1f;

        private int _lastTick;
        private float _time;
        
        private readonly ReactiveProperty<bool> _inProgress = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<float> _normalizedStamina = new ReactiveProperty<float>();
        
        public static readonly int DefaultPriority = 100;
        
        public override float GetPriority(KCC kcc) => DefaultPriority;

        public ReadOnlyReactiveProperty<bool> InProgress => _inProgress;
        public ReadOnlyReactiveProperty<float> StaminaNormalized => _normalizedStamina;

        public void Execute(AfterMoveStep stage, KCC kcc, KCCData data)
        {
            _inProgress.Value = data.Stamina < 100f;

            if (data.Sprint && data.DesiredVelocity != Vector3.zero)
            {
                _time = data.Time;
                data.Stamina += -sprintDrain * data.DeltaTime;
            }

            if (data.HasJumped && _lastTick != data.Tick)
            {
                data.Stamina += -jumpDrain;
                _time = data.Time;
            }
            
            if (data.Time > _time + timeToGain)
                data.Stamina += staminaGain * data.DeltaTime;
            
            data.Stamina = Mathf.Clamp(data.Stamina, bounds.x, bounds.y);
            _normalizedStamina.Value = Mathf.InverseLerp(bounds.x, bounds.y, data.Stamina);
            
            if(data.Stamina == 0f)
                kcc.SetSprint(false);
            
            _lastTick = data.Tick;
        }

        public bool EvaluateJump(KCCData data)
        {
            return data.Stamina >= jumpDrain;
        }
    }
}