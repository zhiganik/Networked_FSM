using System.Collections.Generic;
using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.FSM.Data;
using Shakhtarsk.Characters.Player.FSM.Fall;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Falling : PlayerStateBehaviour
    {
        [SerializeField] private QuickFall quickFall;
        [SerializeField] private HardLand hardLand;
        
        private PlayerStateMachine _fallingMachine;

        protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
        {
            _fallingMachine = new PlayerStateMachine("Falling Machine", this, PlayerMechanism, Animator, 
                KccStatesData, quickFall, hardLand);
            stateMachines.Add(_fallingMachine);
        }
        
        protected override bool CanEnterState()
        {
            return !PlayerMechanism.Kcc.RenderData.IsGrounded && PlayerMechanism.CurrentFallInstruction.CanFall;
        }

        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return hardLand.IsFinished;
        }

        protected override void OnEnterState()
        {
            _fallingMachine.ForceActivateState(quickFall, true);
        }

        protected override void OnExitState()
        {
            PlayerMechanism.CurrentFallInstruction = new FallInstruction(true);
        }
    }
}