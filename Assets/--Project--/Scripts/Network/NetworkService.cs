using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace __Project__.Scripts.Network
{
    public class NetworkService : MonoBehaviour, INetworkService
    {
        [SerializeField] private NetworkConfiguration networkConfiguration;
        [Header("References"), Space]
        [SerializeField] private NetworkRunner runnerPrefab;
        [SerializeField] private NetworkSceneManagerDefault networkSceneManager;

        private InternetConnection _internetConnection;
        private NetworkRunner _myRunner;
        private NetworkEvents _events;

        private void Awake()
        {
            CreateRunner();
        }

        [Inject]
        private void Construct(InternetConnection internetConnection)
        {
            _internetConnection = internetConnection;
        }

        private void CreateRunner()
        {
            _myRunner = Instantiate(runnerPrefab, transform);
            _events = !_myRunner.TryGetComponent<NetworkEvents>(out var component) ? 
                _myRunner.gameObject.AddComponent<NetworkEvents>() : component;
            
            _myRunner.ProvideInput = true;
            SubscribeRunnerEvents();
        }

        private void SubscribeRunnerEvents()
        {
            _events.PlayerJoined.AddListener(OnPlayerJoin);
            _events.OnShutdown.AddListener(OnShutdown);
            _events.PlayerLeft.AddListener(OnPlayerLeft);
            _events.OnConnectedToServer.AddListener(OnConnectedToServer);
        }

        private void UnSubscribeRunnerEvents()
        {
            _events.PlayerJoined.RemoveListener(OnPlayerJoin);
            _events.OnShutdown.RemoveListener(OnShutdown);
            _events.PlayerLeft.RemoveListener(OnPlayerLeft);
            _events.OnConnectedToServer.RemoveListener(OnConnectedToServer);
        }

        public async Task<bool> ConnectToRoom(string room)
        {
            if (!await _internetConnection.HasInternet()) return false;
            
            var activeScene = SceneManager.GetActiveScene();
            var sceneRef = SceneRef.FromIndex(activeScene.buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            
            if (sceneRef.IsValid)
            {
                sceneInfo.AddSceneRef(sceneRef);
            }
            
            var args = new StartGameArgs()
            {
                GameMode = networkConfiguration.gameMode,
                Scene = sceneInfo,
                SceneManager = networkSceneManager,
                PlayerCount = networkConfiguration.maxPlayerCount,
                SessionName = room,
            };
            
            var result = await _myRunner.StartGame(args);
            return result.Ok;
        }

        public async Task<bool> ConnectToLobby()
        {
            if (!await _internetConnection.HasInternet()) return false;
            
            var result = await _myRunner.JoinSessionLobby(SessionLobby.Custom, networkConfiguration.lobbyId);
            return result.Ok;
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.LogError($"Player {player.PlayerId} left");
        }
        
        private void OnPlayerJoin(NetworkRunner runner, PlayerRef player)
        {
            Debug.LogError($"Player {player.PlayerId} join");
        }

        private void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (runner != _myRunner) return;
            
            UnSubscribeRunnerEvents();
            
            if(_myRunner is not null)
                Destroy(_myRunner.gameObject);
            
            CreateRunner();
        }

        private void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.LogError($"Connected to server");
        }
    }
}
