using Fusion;
using UnityEngine;

namespace Shakhtarsk.Network
{
    public class NetworkRunnerHandler : MonoBehaviour
    {
        public NetworkRunner Runner => runner;
        public NetworkEvents Events => events;
        
        public INetworkSceneManager SceneManager
        {
            get
            {
                if (_sceneManager == null && !TryGetComponent(out _sceneManager))
                {
                    _sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
                }

                return _sceneManager;
            }
        }

        public INetworkObjectProvider ObjectProvider
        {
            get
            {
                if (_objectProvider == null && !TryGetComponent(out _objectProvider))
                {
                    _objectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>();
                }

                return _objectProvider;
            }
        }

        [SerializeField] private NetworkRunner runner;
        [SerializeField] private NetworkEvents events;

        private INetworkObjectProvider _objectProvider;
        private INetworkSceneManager _sceneManager;
    }
}