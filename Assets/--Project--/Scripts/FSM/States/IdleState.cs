using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class IdleState : BaseState
    {
        public IdleState(NetworkPlayer player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Animator.CrossFade("Idle", TransitionDuration);
        }
    }
}