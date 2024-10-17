using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Falling : BaseState
    {
        private readonly float _speed = 3f;
        
        public Falling(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }

        public override void OnEnter()
        {
            Player.SetSpeed(_speed);
            Animator.CrossFade("Falling", TransitionDuration);
        }
    }
}