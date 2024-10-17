using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Shakhtarsk.Network.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shakhtarsk.Network
{
    public class NetworkManager : MonoBehaviour
    {
        [Header("General")] 
        [SerializeField] private NetworkRunnerHandler runnerPrefab;
        [SerializeField] private SceneReference startScene;
        
        [HideInInspector]
        [SerializeField] private bool isDebug;

        [Header("Settings")]
        [Tooltip("Number of players in the game")]
        [SerializeField, Range(0,20)] private int additionalPlayerCount;
        
        private NetworkRunnerHandler _runnerHandler;
        private Dictionary<NetworkRunner, NetworkedEvents> _networkedEventsMap;
        private int _buildIndex;

        public bool IsConnectedToRoom => _runnerHandler.Runner.IsInSession;

        private string LOBBY_ID = "DEBUG";
        
        private void Awake()
        {
            Init();
            isDebug = NetworkProjectConfig.Global.PeerMode == NetworkProjectConfig.PeerModes.Multiple;
        }
        
        private async void Init()
        {
            _runnerHandler = Instantiate(runnerPrefab, transform);
            _runnerHandler.Runner.ProvideInput = true;
            
            var scene = SceneManager.GetSceneByPath(startScene.ScenePath);
            _buildIndex = scene.buildIndex;
            _networkedEventsMap = new Dictionary<NetworkRunner, NetworkedEvents>();
            _networkedEventsMap[_runnerHandler.Runner] = new NetworkedEvents(_runnerHandler.Events);
        }

        public async Task<bool> JoinSession()
        {
            var result = await _runnerHandler.Runner.JoinSessionLobby(SessionLobby.Shared, LOBBY_ID);
            
            return result.Ok;
        }

        public async Task JoinRoom(string sessionName)
        {
            await Join(_runnerHandler, sessionName);
        }
        
        public async Task CreateRoom()
        {
            var sessionName = Guid.NewGuid().ToString();
            await Join(_runnerHandler, sessionName);

            if(!isDebug) return;
            
            for (int i = 0; i < additionalPlayerCount; i++)
            {
                var runnerContainer = Instantiate(runnerPrefab);
                
                await Join(runnerContainer, sessionName);

                runnerContainer.Runner.SetVisible(false);
                runnerContainer.Runner.ProvideInput = false;
            }
        }

        public async Task LoadScene(SceneReference sceneReference, LoadSceneMode mode)
        {
            if(!_runnerHandler.Runner.IsSceneAuthority) return;
            
            var sceneName = Path.GetFileNameWithoutExtension(sceneReference.ScenePath);
            
            await _runnerHandler.Runner.LoadScene(sceneName, mode);
        }

        private async Task Join(NetworkRunnerHandler networkRunnerHandler, string sessionName)
        {
            var sceneRef = SceneRef.FromIndex(_buildIndex);
            
            await networkRunnerHandler.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                Scene = sceneRef,
                SessionName = sessionName,
                Address = NetAddress.Any(),
                SceneManager = networkRunnerHandler.SceneManager,
                ObjectProvider = networkRunnerHandler.ObjectProvider
            });

            if (!_networkedEventsMap.ContainsKey(networkRunnerHandler.Runner))
            {
                _networkedEventsMap.Add(networkRunnerHandler.Runner, new NetworkedEvents(networkRunnerHandler.Events));
            }
        }

        public void SubscribeToRunnerEvents<T>(Action<T> eventCallback, NetworkRunner networkRunner = null)
        {
            networkRunner ??= _runnerHandler.Runner;
            
            if (_networkedEventsMap.TryGetValue(networkRunner, out var events))
            {
                events.GetSender<T>().Subscribe(eventCallback);
            }
        }

        public void UnSubscribeFromRunnerEvents<T>(Action<T> eventCallback, NetworkRunner networkRunner = null)
        {
            networkRunner ??= _runnerHandler.Runner;
            
            if (_networkedEventsMap.TryGetValue(networkRunner, out var events))
            {
                events.GetSender<T>().Unsubscribe(eventCallback);
            }
        }
    }
}