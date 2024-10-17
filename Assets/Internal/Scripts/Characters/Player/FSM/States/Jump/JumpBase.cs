using System;
using Shakhtarsk.Characters.Player.Features.Processors;
using Shakhtarsk.Characters.Player.FSM.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    [Serializable]
    public abstract class JumpBase : PlayerState
    {
        [SerializeField, Range(0, 1f)] private float transitionDuration = 0.2f;
        [SerializeField, Range(0, 5f)] private float jumpHeight = 1.5f;
        [SerializeField, Range(0, 5f)] private float jumpLength = 1f;
        [SerializeField] private string stateName;

        private float _calculatedVelocity;

        protected Vector3 StartPos;

        public bool Performed { get; private set; }

        protected override void OnInitialize()
        {
            _calculatedVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        }

        protected void CalculateForceForY(ref Vector3 direction)
        {
            direction.y = _calculatedVelocity * PlayerMechanism.Kcc.Rigidbody.mass;
        }
        
        protected void CalculateForceForXZ(ref Vector3 direction)
        {
            var currentSpeed = PlayerMechanism.Kcc.Data.RealSpeed * jumpLength;
            direction.x *= currentSpeed;
            direction.z *= currentSpeed;
        }
        
        protected override void OnEnterStateRender()
        {
            Animator.CrossFade(stateName, transitionDuration);
        }

        protected void Jump(Vector3 force)
        {
            StartPos = PlayerMechanism.Kcc.Data.BasePosition;
            PlayerMechanism.CurrentFallInstruction = GetFallInstruction();
            PlayerMechanism.Kcc.Jump(force);
        }

        protected abstract FallInstruction GetFallInstruction();
        
        protected abstract bool IsPerformed();

        protected override void OnFixedUpdate()
        {
            Performed = IsPerformed();
        }
    }
}