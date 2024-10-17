using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class PlayerStateMachine : StateMachine<PlayerState>
    {
        public PlayerStateMachine(string name, PlayerStateBehaviour parentState, PlayerMechanism playerMechanism,
            Animator animator, KCCStatesData kccStatesData, params PlayerState[] states) : base(name, states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                var state = states[i];

                state.ParentState = parentState;
                state.Animator = animator;
                state.KccStatesData = kccStatesData;
                state.PlayerMechanism = playerMechanism;
            }
        }
    }
}