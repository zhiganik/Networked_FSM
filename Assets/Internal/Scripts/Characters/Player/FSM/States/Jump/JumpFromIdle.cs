using System;
using Shakhtarsk.Characters.Player.FSM.Data;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.FSM
{
    [Serializable]
    public class JumpFromIdle : JumpBase
    {
        private Vector3 _lastPosition;

        
        protected override void OnEnterState()
        {
            var direction = Vector3.up;
            CalculateForceForY(ref direction);
            Jump(direction);
        }

        protected override FallInstruction GetFallInstruction()
        {
            return new FallInstruction(true);
        }

        protected override bool IsPerformed()
        {
            var position = PlayerMechanism.Kcc.Data.BasePosition;

            if (position.y < _lastPosition.y)
            {
                return true;
            }
            
            _lastPosition = position;
            return false;
        }
    }
}