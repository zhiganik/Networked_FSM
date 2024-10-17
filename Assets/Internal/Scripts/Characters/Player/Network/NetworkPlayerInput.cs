using Fusion;
using UnityEngine;

namespace Shakhtarsk.Characters.Player.Network
{
    public enum NetworkButtonsFlags
    {
        Jump,
        Crouch,
        Sprint,
        Interact,
        Release
    }
    
    public struct NetworkPlayerInput : INetworkInput
    {
        public Vector2 Input;
        public Vector2 Mouse;
        public NetworkButtons Buttons;
    }
}