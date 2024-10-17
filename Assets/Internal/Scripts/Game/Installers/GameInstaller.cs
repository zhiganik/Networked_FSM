using Zenject;

namespace Shakhtarsk.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle().NonLazy();
        }
    }
}