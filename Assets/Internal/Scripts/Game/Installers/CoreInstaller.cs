using Shakhtarsk.Characters.Player.Network;
using Shakhtarsk.Input;
using Shakhtarsk.Interactions;
using UnityEngine;
using Zenject;

namespace Shakhtarsk.Game.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private InteractionMessagesMap messagesMap;
        [SerializeField] private InteractionHub interactionHub;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.Bind<InteractionMessagesMap>().FromInstance(messagesMap);
            Container.Bind<InteractionHub>().FromInstance(interactionHub).AsSingle();
            Container.Bind<Spawner>().FromInstance(spawner);
        }
    }
}