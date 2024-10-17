using System;
using Fusion.Addons.FSM;
using Shakhtarsk.Characters.Player.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    [Serializable]
    public class PlayerState : State<PlayerState>
    {
        [HideInInspector] public PlayerStateBehaviour ParentState;
        [HideInInspector] public PlayerMechanism PlayerMechanism;
        [HideInInspector] public Animator Animator;
        [HideInInspector] public KCCStatesData KccStatesData;
    }
}