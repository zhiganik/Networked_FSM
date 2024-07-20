using __Project__.Scripts.Input;
using Fusion;
using UnityEngine;
using Zenject;

namespace __Project__.Scripts.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class Spawner : NetworkBehaviour
    {
        [SerializeField] private NetworkPlayer playerPrefab;
        [SerializeField] private Transform spawnPoint;
        
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        public override void Spawned()
        {
            var player = Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.Initialize(_inputService);
        }
    }
}