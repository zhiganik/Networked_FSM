using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class WalkState : BaseState
    {
        
        
        public WalkState(NetworkPlayer player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Animator.CrossFade("Walk", TransitionDuration);
        }
    }
}