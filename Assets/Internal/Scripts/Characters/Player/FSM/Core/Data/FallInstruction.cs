using Fusion;

namespace Shakhtarsk.Characters.Player.FSM.Data
{
    public struct FallInstruction : INetworkStruct
    {
        public NetworkBool CanFall { get; private set; }
        
        public FallInstruction(NetworkBool canFall)
        {
            CanFall = canFall;
        }
    }
}