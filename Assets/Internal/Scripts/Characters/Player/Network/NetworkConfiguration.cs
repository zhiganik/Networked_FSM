using System;
using Fusion;

namespace Shakhtarsk.Characters.Player.Network
{
    [Serializable]
    public struct NetworkConfiguration
    {
        public int maxPlayerCount;
        public string lobbyId;
        public GameMode gameMode;
    }
}