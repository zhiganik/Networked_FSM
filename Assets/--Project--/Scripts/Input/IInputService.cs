using System;
using UnityEngine;

namespace __Project__.Scripts.Input
{
    public interface IInputService
    {
        public event Action<Vector2> OnMove;
        public event Action<Vector2> OnLook;
        public event Action OnJump;
    }
}