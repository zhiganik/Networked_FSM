using System;
using Fusion.Addons.FSM;

namespace Shakhtarsk.Characters.Player.FSM
{
    [Serializable]
    public class JumpPreparation : PlayerState
    {
        protected override void OnEnterState()
        {
            var previousParentState = ParentState.Machine.PreviousState;
            
            if (previousParentState is Idle)
            {
                Machine.TryActivateState<JumpFromIdle>();
                return;
            }
            
            if (previousParentState is Walk)
            {
                Machine.TryActivateState<JumpFromWalking>();
                return;
            }
            
            if (previousParentState is Sprint)
            {
                Machine.TryActivateState<JumpFromSprint>();
            }
        }
    }
}