using __Project__.Scripts.FSM;
using __Project__.Scripts.FSM.Predicates;
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
            _stateMachine = new StateMachine();
            
            var walkState = new Locomotion(player, animator);
            var idleState = new IdleState(player, animator);
            var jumpState = new JumpState(player, animator);
            var fallingState = new FallingState(player, animator);
            
            At(idleState, walkState, new FuncPredicate(() => player.IsMove));
            At(walkState, idleState, new FuncPredicate(() => !player.IsMove));
            
            At(idleState, jumpState, new FuncPredicate(() => player.IsJump && player.IsGrounded));
            At(walkState, jumpState, new FuncPredicate(() => player.IsJump && player.IsGrounded));
            
            Any(fallingState, new FuncPredicate(() => !player.IsGrounded && !player.IsJump));
            
            At(fallingState, idleState, new FuncPredicate(() => player.IsGrounded));
            At(fallingState, walkState, new FuncPredicate(() => player.IsGrounded && player.IsMove));
            
            _stateMachine.SetState(idleState);
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