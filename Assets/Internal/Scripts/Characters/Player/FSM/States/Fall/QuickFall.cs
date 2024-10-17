using System;
using Fusion.Addons.FSM;

namespace Shakhtarsk.Characters.Player.FSM.Fall
{
    [Serializable]
    public class QuickFall : PlayerState
    {
        protected override void OnEnterStateRender()
        {
            PlayerMechanism.SetCharacterHeight(KccStatesData.JumpHeight);
            Animator.CrossFade("Falling", 0.5f);
        }
        
        protected override void OnFixedUpdate()
        {
            if (!PlayerMechanism.Kcc.RenderData.IsGrounded) return;

            if (Machine.StateTime > 0.8f)
            {
                Machine.ForceActivateState<HardLand>();
            }
            else
            {
                ParentState.Machine.ForceDeactivateState(ParentState.StateId);
            }
        }
    }
}