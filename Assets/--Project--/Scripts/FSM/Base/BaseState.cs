using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM
{
    public abstract class BaseState : IState
    {
        protected readonly NetworkPlayer Player;
        protected readonly Animator Animator;

        protected const float TransitionDuration = 0.35f;
        
        protected static readonly int VelocityX = Animator.StringToHash("VelocityX");
        protected static readonly int VelocityY = Animator.StringToHash("VelocityY");
        
        protected BaseState(NetworkPlayer player, Animator animator)
        {
            Player = player;
            Animator = animator;
        }

        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void Update()
        {
            Player.HandleFloor();
            Player.HandleGravity();
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }
    }
}