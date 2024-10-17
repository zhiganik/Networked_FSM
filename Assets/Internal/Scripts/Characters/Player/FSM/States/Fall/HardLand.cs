using System;

namespace Shakhtarsk.Characters.Player.FSM.Fall
{
    [Serializable]
    public class HardLand : PlayerState
    {
        public bool IsFinished { get; set; }

        protected override void OnEnterStateRender()
        {
            PlayerMechanism.SetCharacterHeight(KccStatesData.DefaultHeight);
            Animator.CrossFade("HardLanding", 0.15f);
        }
        
        protected override void OnEnterState()
        {
            PlayerMechanism.IsLocked = true;
        }

        protected override void OnFixedUpdate()
        {
            if (!PlayerMechanism.Kcc.Data.IsGrounded)
            {
                PlayerMechanism.IsLocked = false;
                ParentState.Machine.ForceDeactivateState(ParentState.StateId);
            }
            
            if (Machine.StateTime > 1f)
            {
                PlayerMechanism.IsLocked = false;
                IsFinished = true;
            }
        }
        
        protected override void OnExitState()
        {
            PlayerMechanism.IsLocked = false;
            IsFinished = false;
        }
    }
}