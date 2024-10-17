using __Project__.Scripts.Input;
using Zenject;

namespace __Project__.Scripts.Installers
{
    public class CoreInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputService>()
                .AsSingle();
        }
    }
}