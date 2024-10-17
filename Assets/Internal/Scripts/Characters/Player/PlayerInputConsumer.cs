

using Fusion;
using Fusion.Addons.KCC;
using R3;
using Shakhtarsk.Characters.Player.Network;
using Shakhtarsk.Input;
using Shakhtarsk.Network;
using Shakhtarsk.Network.Events;
using UnityEngine;

namespace Shakhtarsk.Characters.Player
{
    public class PlayerInputConsumer : NetworkBehaviour, IPlayerComponent
    {
        private ReactiveProperty<bool> _inputProvided;
        
        private IInputService _inputService;
        private NetworkManager _networkManager;
        private NetworkPlayerInput _playerInput;
        private int _lastInputFrame;

        private readonly Vector2Accumulator _lookRotationAccumulator = new Vector2Accumulator(0.02f, true);

        public void Initialize(IInputService inputService, NetworkManager networkManager)
        {
            _inputProvided = new ReactiveProperty<bool>();
            
            _inputService = inputService;
            _networkManager = networkManager;
            
            _inputProvided.Subscribe(ActivateInput).AddTo(this);
        }

        private void Update()
        {
            if(!HasStateAuthority && !HasInputAuthority) return;
            
            _inputProvided.Value = Runner.ProvideInput;
        }

        private void ActivateInput(bool state)
        {
            _inputService.ActivateInput(state);

            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = state;
            
            if (state)
            {
                _networkManager.SubscribeToRunnerEvents<NetworkPlayerInputArgs>(CollectInput, Runner);
                return;
            }
            
            _networkManager.UnSubscribeFromRunnerEvents<NetworkPlayerInputArgs>(CollectInput, Runner);
        }

        private void CollectInput(NetworkPlayerInputArgs args)
        {
            if (!HasPermission()) return;
            
            TryAccumulateInput();
            _playerInput.Mouse = _lookRotationAccumulator.ConsumeTickAligned(args.Runner);
            args.Input.Set(_playerInput);
            _playerInput = default;
        }
        
        public override void Render()
        {
            TryAccumulateInput();
        }

        private void TryAccumulateInput()
        {
            int currentFrame = Time.frameCount;
            
            if (currentFrame == _lastInputFrame)
                return;

            _lastInputFrame = currentFrame;
            
            if (!HasPermission()) return;

            var delta = _inputService.Look.CurrentValue;

            _lookRotationAccumulator.Accumulate(new Vector2(-delta.y, delta.x) * 0.25f);
            
            _playerInput.Input = _inputService.Move.CurrentValue.normalized;
            
            _playerInput.Buttons.Set(NetworkButtonsFlags.Jump, _inputService.IsJump.CurrentValue);
            _playerInput.Buttons.Set(NetworkButtonsFlags.Sprint, _inputService.IsSprint.CurrentValue);
            _playerInput.Buttons.Set(NetworkButtonsFlags.Crouch, _inputService.IsCrouch.CurrentValue);
            _playerInput.Buttons.Set(NetworkButtonsFlags.Interact, _inputService.IsInteract.CurrentValue);
            _playerInput.Buttons.Set(NetworkButtonsFlags.Release, _inputService.IsRelease.CurrentValue);
        }

        private bool HasPermission()
        {
            return HasInputAuthority && HasStateAuthority && Runner.ProvideInput;
        }
    }
}