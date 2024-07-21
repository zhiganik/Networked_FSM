using Fusion;
using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.FSM
{
    public abstract class BaseState : IState
    {
        protected readonly NetworkPlayer Player;
        protected readonly Animator Animator;

        protected const float TransitionDuration = 0.1f;
        
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
            // noop
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