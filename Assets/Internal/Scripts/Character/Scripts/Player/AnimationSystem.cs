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
            
            var walkState = new Run(player, animator);
            var fallingState = new Falling(player, animator);
            var idleState = new Idle(player, animator);
            var jumpState = new Jump(player, animator);
            var sprintState = new Sprint(player, animator);
            var crunchState = new Crouch(player, animator);

            At(idleState, walkState, new FuncPredicate(() => player.IsMoving));
            At(walkState, idleState, new FuncPredicate(() => !player.IsMoving));
            
            At(walkState, sprintState, new FuncPredicate(() => player.IsMoving && player.IsSprint));
            At(sprintState, walkState, new FuncPredicate(() => player.IsMoving && !player.IsSprint));
            
            Any(jumpState, new FuncPredicate(() => player.IsGrounded && player.IsJumping));
            
            Any(fallingState, new FuncPredicate(() => !player.IsGrounded && !player.IsJumping));
            Any(crunchState, new FuncPredicate(() => player.IsGrounded && player.IsCrouch));
            
            At(crunchState, idleState, new FuncPredicate(() => !player.IsCrouch && !player.IsMoving));
            At(crunchState, walkState, new FuncPredicate(() => !player.IsCrouch && player.IsMoving));
            
            At(fallingState, idleState, new FuncPredicate(() => player.IsGrounded));
            At(fallingState, walkState, new FuncPredicate(() => player.IsGrounded && player.IsMoving));
            
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