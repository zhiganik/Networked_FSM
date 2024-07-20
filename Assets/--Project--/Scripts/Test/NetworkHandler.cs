using __Project__.Scripts.Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace __Project__.Scripts.Test
{
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] private string roomName;
        
        private INetworkService _networkService;

        [Inject]
        private void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        private async void Start()
        {
            if(!await _networkService.ConnectToLobby()) return;

            await _networkService.ConnectToRoom(roomName);
            await _networkService.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}