using System;
using Shakhtarsk.Characters.Player.FSM.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    [Serializable]
    public class JumpFromSprint : JumpBase
    {
        protected override void OnEnterState()
        {
            var currentInput = PlayerMechanism.CurrentInput;
            var direction = new Vector3(currentInput.x, 1f, currentInput.y);
            CalculateForceForY(ref direction);
            CalculateForceForXZ(ref direction);
            Vector3 rotatedPoint = PlayerMechanism.Kcc.Data.TransformRotation * direction;
            Jump(rotatedPoint);
        }
        
        protected override FallInstruction GetFallInstruction()
        {
            return new FallInstruction(false);
        }
        protected override bool IsPerformed()
        {
            var targetPos = PlayerMechanism.Kcc.Data.DesiredPosition;

            if (StartPos.y > targetPos.y)
            {
                PlayerMechanism.CurrentFallInstruction = new FallInstruction(true);
                return true;
            }
            
            return PlayerMechanism.Kcc.Data.IsGrounded;
        }
    }
}