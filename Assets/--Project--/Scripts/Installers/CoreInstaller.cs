using __Project__.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace __Project__.Scripts.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput playerInput;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputService>()
                .AsSingle()
                .WithArguments(playerInput);
        }
    }
}