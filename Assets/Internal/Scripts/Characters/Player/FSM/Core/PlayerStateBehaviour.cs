using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    public class PlayerStateBehaviour : StateBehaviour<PlayerStateBehaviour>
    {
        [HideInInspector] public PlayerMechanism PlayerMechanism;
        [HideInInspector] public Animator Animator;
        [HideInInspector] public KCCStatesData KccStatesData;
    }
}