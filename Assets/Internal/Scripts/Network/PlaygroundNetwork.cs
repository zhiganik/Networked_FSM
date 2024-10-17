using UnityEngine;
using Zenject;

namespace Shakhtarsk.Network
{
    public class PlaygroundNetwork : MonoBehaviour
    {
        private NetworkManager _network;
        
        [Inject]
        private void Construct(NetworkManager network)
        {
            _network = network;
        }

        private void Awake()
        {
            Init();
        }

        private async void Init()
        {
            if (_network.IsConnectedToRoom) return;
            
            await _network.JoinSession();
            await _network.CreateRoom();
        }
    }
}