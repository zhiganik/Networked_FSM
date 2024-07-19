using __Project__.Scripts.Network;
using UnityEngine;
using Zenject;

namespace __Project__.Scripts.Installers
{
    public class NetworkInstaller : MonoInstaller
    {
        [SerializeField] private NetworkService networkService;
        
        public override void InstallBindings()
        {
            var networkServiceInstance = Container.InstantiatePrefabForComponent<NetworkService>(networkService);
            Container.BindInterfacesTo<NetworkService>()
                .FromInstance(networkServiceInstance)
                .AsSingle();
        }
    }
}