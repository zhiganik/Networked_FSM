using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Jump : BaseState
    {
        private readonly float _speed = 1f;
        
        private const float CharacterCenterY = 0.94f;
        private const float CharacterHeight = 1.2f;
        
        public Jump(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }

        public override void OnEnter()
        {
            Player.CharacterController.height = CharacterHeight;
            Player.CharacterController.center = Vector3.up * CharacterCenterY;
            
            Animator.CrossFade("Jump", TransitionDuration);
        }

        public override void Update()
        {
            base.Update();
            Player.HandleMovement(_speed);
            Player.HandleRotation(_speed);
        }
    }
}