using Fusion;
using Shakhtarsk.Characters.Player;
using Shakhtarsk.Network;
using Shakhtarsk.Network.Events;
using UnityEngine;
using Zenject;

namespace Shakhtarsk.Interactions
{
    public class InteractionHub : NetworkBehaviour
    {
        [Networked, Capacity(10)] 
        public NetworkDictionary<NetworkBehaviourId, NetworkBehaviourId> playersCurrentItems { get; }
        
        private PlayerResolver _playerResolver;
        private NetworkManager _networkManager;
        
        [Inject]
        private void Construct(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }
        
        public override void Spawned()
        {
            _networkManager.SubscribeToRunnerEvents<PlayerLeftEvent>(PlayerLeft);

            foreach (var (player, grabbable) in playersCurrentItems)
            {
                Interact(player, grabbable);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _networkManager.UnSubscribeFromRunnerEvents<PlayerLeftEvent>(PlayerLeft);
        }

        private void PlayerLeft(PlayerLeftEvent playerLeft)
        {
            if (Object.StateAuthority == playerLeft.PlayerRef)
            {
                Object.RequestStateAuthority();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void Rpc_Interact(NetworkBehaviourId playerResolverId, NetworkBehaviourId grabbableId)
        {
            Interact(playerResolverId, grabbableId);
        }

        private void Interact(NetworkBehaviourId playerResolverId, NetworkBehaviourId grabbableId)
        {
            if (!Runner.TryFindBehaviour(playerResolverId, out PlayerResolver playerResolver)) return;

            if (Runner.TryFindBehaviour(grabbableId, out GrabbableObject grabbableObject))
            {
                Debug.LogError($"Player: {playerResolver.Object.Runner.LocalPlayer} GRABBED object with name {grabbableObject.name} ");
                grabbableObject.Interact(playerResolver);

                if (!playersCurrentItems.ContainsKey(playerResolverId))
                {
                    playersCurrentItems.Add(playerResolverId, grabbableId);
                }
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void Rpc_Release(NetworkBehaviourId playerResolverId, NetworkBehaviourId grabbableId)
        {
            if (!Runner.TryFindBehaviour(playerResolverId, out PlayerResolver playerResolver)) return;
            
            if (Runner.TryFindBehaviour(grabbableId, out GrabbableObject grabbableObject))
            {
                Debug.LogError($"Player: {playerResolver.Object.Runner.LocalPlayer} RELEASED object with name {grabbableObject.name} ");
                grabbableObject.Release(playerResolver);
                
                if (playersCurrentItems.ContainsKey(playerResolverId))
                {
                    playersCurrentItems.Remove(playerResolverId);
                }
            }
        }
    }
}