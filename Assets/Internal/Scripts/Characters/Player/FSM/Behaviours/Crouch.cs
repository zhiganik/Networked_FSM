using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Crouch : PlayerStateBehaviour
    {
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityY = Animator.StringToHash("VelocityY");
        
        protected override bool CanEnterState()
        {
            return PlayerMechanism.Kcc.Data.Crouch;
        }

        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return !PlayerMechanism.Kcc.Data.Crouch;
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Crouching...");
            PlayerMechanism.SetCharacterHeight(KccStatesData.CrouchHeight);
            Animator.CrossFade("Crouch", 0.1f);
        }
        
        protected override void OnExitStateRender()
        {
            Debug.Log("Crouching finished");
        }

        protected override void OnRender()
        {
            Animator.SetFloat(VelocityX, PlayerMechanism.CurrentInput.x, 0.25f, Time.deltaTime);
            Animator.SetFloat(VelocityY, PlayerMechanism.CurrentInput.y, 0.25f, Time.deltaTime);
        }
    }
}