using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Crouch : BaseState
    {
        private readonly float _speed = 1;
        private readonly float _rotationSpeed = 5;
        
        public Crouch(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }

        public override void OnEnter()
        {
            Animator.CrossFade("Crouch", TransitionDuration);
        }
        
        public override void Update()
        {
            base.Update();
            Player.HandleMovement(_speed);
            Player.HandleRotation(_rotationSpeed);
            
            Animator.SetFloat(VelocityX, Player.CurrentInput.x);
            Animator.SetFloat(VelocityY, Player.CurrentInput.y);
        }
    }
}