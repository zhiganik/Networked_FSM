using System;
using Fusion;
using Shakhtarsk.Input;
using Shakhtarsk.Interactions;
using Shakhtarsk.Network;
using UnityEngine;
using Zenject;

namespace Shakhtarsk.Characters.Player.Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class Spawner : NetworkBehaviour
    {
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Camera targetCamera;
        
        private IInputService _inputService;
        private NetworkManager _networkManager;
        private InteractionHub _interactionHub;

        public event Action<PlayerResolver> PlayerSpawned; 
        
        [Inject]
        private void Construct(IInputService inputService, NetworkManager networkManager, InteractionHub interactionHub)
        {
            _inputService = inputService;
            _networkManager = networkManager;
            _interactionHub = interactionHub;
        }
        
        public override void Spawned()
        {
            var player = Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation, Runner.LocalPlayer);
            Runner.SetPlayerObject(Runner.LocalPlayer, player);
            
            var resolver = player.GetComponent<PlayerResolver>();
            var consumer = player.GetComponent<PlayerInputConsumer>();
            var mechanism = player.GetComponent<PlayerMechanism>();
            var interaction = player.GetComponent<PlayerInteraction>();
            
            consumer.Initialize(_inputService, _networkManager);
            mechanism.SetCamera(targetCamera);
            interaction.Initialize(resolver, _interactionHub, targetCamera.transform);
            
            PlayerSpawned?.Invoke(resolver);
        }
    }
}