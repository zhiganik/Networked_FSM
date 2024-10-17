using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Idle : PlayerStateBehaviour
    {
        protected override bool CanEnterState()
        {
            return PlayerMechanism.CurrentInput == Vector2.zero && PlayerMechanism.Kcc.RenderData.IsGrounded;;
        }

        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return PlayerMechanism.CurrentInput != Vector2.zero || nextState.Priority > Priority;
        }

        protected override void OnEnterStateRender()
        {
            Debug.Log("Idling...");
            PlayerMechanism.SetCharacterHeight(KccStatesData.DefaultHeight);
            Animator.CrossFade("Idle", 0.25f);
        }
        
        protected override void OnExitStateRender()
        {
            Debug.Log("Idling finished");
        }
    }
}