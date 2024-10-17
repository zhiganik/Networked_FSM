using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.Data;
using Shakhtarsk.Input;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class PlayerBehaviourMachine : StateMachine<PlayerStateBehaviour>
    {
        public PlayerBehaviourMachine(string name, PlayerMechanism playerMechanism, Animator animator, KCCStatesData kccStatesData,
            params PlayerStateBehaviour[] states) : base(name, states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                var state = states[i];

                state.PlayerMechanism = playerMechanism;
                state.Animator = animator;
                state.KccStatesData = kccStatesData;
            }
        }
    }
}