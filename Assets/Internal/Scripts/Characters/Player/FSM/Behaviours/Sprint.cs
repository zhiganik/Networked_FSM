using Shakhtarsk.Characters.Player.FSM.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Sprint : PlayerStateBehaviour 
    {
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityY = Animator.StringToHash("VelocityY");
        
        protected override bool CanEnterState()
        {
            return PlayerMechanism.CurrentInput != Vector2.zero &&
                   PlayerMechanism.IsSprint && 
                   PlayerMechanism.Kcc.Data.IsGrounded;
        }

        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return !PlayerMechanism.IsSprint || 
                   PlayerMechanism.CurrentInput == Vector2.zero || 
                   !PlayerMechanism.Kcc.Data.IsGrounded ||
                   nextState.Priority > Priority;
        }

        protected override void OnEnterStateRender()
        {
            PlayerMechanism.SetCharacterHeight(KccStatesData.DefaultHeight);
            Animator.CrossFade("Sprint", 0.1f);
        }
        
        protected override void OnEnterState()
        {
            PlayerMechanism.CurrentFallInstruction = new FallInstruction(true);
        }

        protected override void OnRender()
        {
            Animator.SetFloat(VelocityX, PlayerMechanism.CurrentInput.x, 0.25f, Time.deltaTime);
            Animator.SetFloat(VelocityY, PlayerMechanism.CurrentInput.y, 0.25f, Time.deltaTime);
        }
    }
}