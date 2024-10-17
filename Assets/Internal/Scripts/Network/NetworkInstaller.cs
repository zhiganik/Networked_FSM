using UnityEngine;
using Zenject;

namespace Shakhtarsk.Network
{
    public class NetworkInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private NetworkManager networkManager;
        
        public override void InstallBindings()
        {
            Container.Bind<NetworkManager>().FromInstance(networkManager).AsSingle().NonLazy().IfNotBound();
        }

        public void Initialize()
        {
            InstallBindings();
        }
    }
}