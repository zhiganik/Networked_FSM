using System;
using UnityEngine;

namespace __Project__.Scripts.Input
{
    public interface IInputService
    {
        public event Action<Vector2> OnMovePerformed;
        public event Action OnMoveCanceled;
    }
}