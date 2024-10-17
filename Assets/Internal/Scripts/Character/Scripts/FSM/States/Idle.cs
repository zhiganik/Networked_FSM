using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Idle : BaseState
    {
        private readonly float _rotationSpeed = 2;
        
        private const float CharacterCenterY = 1f;
        private const float CharacterHeight = 1.79f;
        
        public Idle(NetworkPlayer player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Player.CharacterController.height = CharacterHeight;
            Player.CharacterController.center = Vector3.up * CharacterCenterY;
            
            Animator.CrossFade("Idle", TransitionDuration);
        }

        public override void Update()
        {
            Player.HandleRotation(_rotationSpeed);
        }
    }
}