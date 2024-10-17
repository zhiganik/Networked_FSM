using System;
using Fusion;

namespace __Project__.Scripts.Network
{
    [Serializable]
    public struct NetworkConfiguration
    {
        public int maxPlayerCount;
        public string lobbyId;
        public GameMode gameMode;
    }
}