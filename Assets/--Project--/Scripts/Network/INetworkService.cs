using System.Threading.Tasks;

namespace __Project__.Scripts.Network
{
    public interface INetworkService
    {
        public Task<bool> ConnectToRoom(string room);

        public Task<bool> ConnectToLobby();
    }
}