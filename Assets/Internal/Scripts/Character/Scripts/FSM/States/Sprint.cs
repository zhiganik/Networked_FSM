using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Sprint : BaseState
    {
        private readonly float _speed = 4;
        
        public Sprint(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }
        
        public override void OnEnter()
        {
            Player.SetSpeed(_speed);
            Animator.CrossFade("Sprint", TransitionDuration);
        }
        
        public override void Update()
        {
            Animator.SetFloat(VelocityX, Player.CurrentInput.x);
            Animator.SetFloat(VelocityY, Player.CurrentInput.y);
        }
    }
}