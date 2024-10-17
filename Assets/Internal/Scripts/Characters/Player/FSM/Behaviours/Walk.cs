using Shakhtarsk.Characters.Player.FSM.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Walk : PlayerStateBehaviour
    {
        protected static readonly int VelocityX = Animator.StringToHash("VelocityX");
        protected static readonly int VelocityY = Animator.StringToHash("VelocityY");
        
        protected override bool CanEnterState()
        {
            var kccData = PlayerMechanism.Kcc.Data;
            return PlayerMechanism.CurrentInput != Vector2.zero && !kccData.Sprint && kccData.IsGrounded;
        }
        
        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return PlayerMechanism.CurrentInput == Vector2.zero ||
                   nextState.Priority > Priority ||
                   !PlayerMechanism.Kcc.Data.IsGrounded ||
                   PlayerMechanism.Kcc.Data.Sprint;
        }

        protected override void OnEnterStateRender()
        {
            PlayerMechanism.SetCharacterHeight(KccStatesData.DefaultHeight);
            Animator.CrossFade("Run", 0.1f);
        }

        protected override void OnEnterState()
        {
            PlayerMechanism.CurrentFallInstruction = new FallInstruction(true);
        }
        
        protected override void OnRender()
        {
            Animator.SetFloat(VelocityX, PlayerMechanism.CurrentInput.x, 0.15f, Time.deltaTime);
            Animator.SetFloat(VelocityY, PlayerMechanism.CurrentInput.y, 0.15f, Time.deltaTime);
        }
    }
}