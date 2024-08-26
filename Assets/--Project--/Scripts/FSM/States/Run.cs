using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Run : BaseState
    {
        private readonly float _speed = 2;
        private readonly float _rotationSpeed = 5;
        
        private const float CharacterCenterY = 1f;
        private const float CharacterHeight = 1.79f;
        
        public Run(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }

        public override void OnEnter()
        {
            Player.CharacterController.height = CharacterHeight;
            Player.CharacterController.center = Vector3.up * CharacterCenterY;
            
            Animator.CrossFade("Run", TransitionDuration);
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