using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM.States
{
    public class Locomotion : BaseState
    {
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityY = Animator.StringToHash("VelocityY");

        private Vector2 _lastInput;
        
        public Locomotion(NetworkPlayer player, Animator animator) : base(player, animator)
        {
            
        }

        public override void OnEnter()
        {
            Animator.CrossFade("Locomotion", TransitionDuration);
        }
        
        public override void Update()
        {
            Animator.SetFloat(VelocityX, Player.CurrentInput.x);
            Animator.SetFloat(VelocityY, Player.CurrentInput.y);
        }
    }
}