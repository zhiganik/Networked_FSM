using System.Collections.Generic;
using System.Linq;
using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.Features.Processors;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class Jump : PlayerStateBehaviour
    {
        [SerializeField] private JumpPreparation jumpPreparation;
        [SerializeField] private JumpFromIdle jumpFromIdle;
        [SerializeField] private JumpFromWalking jumpFromWalking;
        [SerializeField] private JumpFromSprint jumpFromSprint;

        private PlayerStateMachine _jumpMachine;
        private List<JumpBase> _jumpStates;

        protected override void OnCollectChildStateMachines(List<IStateMachine> stateMachines)
        {
            _jumpMachine = new PlayerStateMachine("Jump Machine", this, PlayerMechanism, Animator, 
                KccStatesData, jumpPreparation, jumpFromIdle, jumpFromWalking, jumpFromSprint);
            stateMachines.Add(_jumpMachine);
        }
        
        protected override bool CanEnterState()
        {
            var staminaProcessor = PlayerMechanism.Kcc.GetProcessor<StaminaProcessor>();
            
            return PlayerMechanism.IsJump && staminaProcessor.EvaluateJump(PlayerMechanism.Kcc.Data);
        }

        protected override bool CanExitState(PlayerStateBehaviour nextState)
        {
            return _jumpStates.Any(j => j.Performed);
        }

        protected override void OnEnterState()
        {
            _jumpStates = new List<JumpBase>() { jumpFromIdle, jumpFromWalking, jumpFromSprint };
            _jumpMachine.ForceActivateState(jumpPreparation, true);
        }

        protected override void OnExitState()
        {
            _jumpStates.Clear();
            _jumpStates = null;
        }
    }
}