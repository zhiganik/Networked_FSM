using __Project__.Scripts.FSM;
using __Project__.Scripts.FSM.States;
using Fusion;
using UnityEngine;
using NetworkPlayer = __Project__.Scripts.Network.NetworkPlayer;

namespace __Project__.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NetworkPlayer))]
    public class AnimationSystem : NetworkBehaviour
    {
        [SerializeField] private NetworkPlayer player;
        [SerializeField] private Animator animator;
        
        private StateMachine _stateMachine;
        
        public override void Spawned()
        {
            var walkState = new WalkState(player, animator);
            var idleState = new IdleState(player, animator);
            
            
            
        }

        public void At(IState from, IState to, IPredicate condition)
        {
            _stateMachine.AddTransition(from,to,condition);
        }

        public void Any(IState to, IPredicate condition)
        {
            _stateMachine.AddAnyTransitions(to, condition);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
    }
}