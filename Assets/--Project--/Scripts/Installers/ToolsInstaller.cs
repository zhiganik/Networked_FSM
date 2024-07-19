using __Project__.Scripts.Network;
using Zenject;

namespace __Project__.Scripts.Installers
{
    public class ToolsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InternetConnection>().AsSingle();
        }
    }
}