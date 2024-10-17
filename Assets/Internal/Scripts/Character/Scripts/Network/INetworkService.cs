using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace __Project__.Scripts.Network
{
    public interface INetworkService
    {
        public Task<bool> ConnectToRoom(string room);
        public Task<bool> ConnectToLobby();
        public Task LoadScene(int index, LoadSceneMode mode);
    }
}