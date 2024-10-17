using System;
using Fusion;
using R3;
using Shakhtarsk.Characters.Player;
using Shakhtarsk.Characters.Player.Network;
using Shakhtarsk.Input;
using Shakhtarsk.Sensors;
using UnityEngine;

namespace Shakhtarsk.Interactions
{
    public class PlayerInteraction : NetworkBehaviour, IPlayerComponent
    {
        [Header("Sensor Settings")] 
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float sphereCastRadius;
        [SerializeField] private float maxCastDistance;

        [Networked] public NetworkButtons PreviousButtons { get; set; }
        
        private CastSensor _castSensor;
        private InteractionHub _interactionHub;
        private PlayerResolver _playerResolver;
        private IInteractable _focusedInteractable;

        private IInteractable _currentInteractable;

        public IInteractable FocusedInteractable => _focusedInteractable;
        
        public event Action<bool> FocusStateChanged = delegate { };
        
        public void Initialize(PlayerResolver playerResolver, InteractionHub interactionHub, Transform targetCamera)
        {
            _interactionHub = interactionHub;
            _playerResolver = playerResolver;
            
            if(!HasStateAuthority) return;
            
            _castSensor = new SphereCastSensor(targetCamera, sphereCastRadius, Runner.GetPhysicsScene(), 
                UnityFrameProvider.PostLateUpdate);

            _castSensor.Filter = typeof(IInteractable);
            _castSensor.LayerMask = layerMask;
            _castSensor.LocalDirection = Vector3.forward;
            _castSensor.MaxDistance = maxCastDistance;

            _castSensor.Detected += OnSensorDetected;
            _castSensor.Lost += OnSensorLost;
        }

        private void OnDrawGizmosSelected()
        {
            _castSensor?.DrawGizmos();
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput<NetworkPlayerInput>(out var playerInput) || !Runner.ProvideInput) return;
            
            var pressed = playerInput.Buttons.GetPressed(PreviousButtons);
            var released = playerInput.Buttons.GetReleased(PreviousButtons);

            if (pressed.IsSet(NetworkButtonsFlags.Interact)) OnInteractPressed();
            if (pressed.IsSet(NetworkButtonsFlags.Release)) OnReleasePressed();
        }

        private void OnSensorDetected(SensorInfo info)
        {
            if (!info.TryGetComponent(out IInteractable interactable) || _focusedInteractable == interactable) return;
            
            _focusedInteractable = interactable;
            _focusedInteractable.Focus();
            
            FocusStateChanged.Invoke(true);
        }

        private void OnSensorLost(SensorInfo info)
        {
            if (!info.TryGetComponent(out IInteractable interactable) || _focusedInteractable != interactable) return;

            UnFocus();
        }

        private void UnFocus()
        {
            _focusedInteractable.UnFocus();
            _focusedInteractable = null;
            
            FocusStateChanged.Invoke(false);
        }

        private void OnInteractPressed()
        {
            if (_focusedInteractable == null) return;
            
            _interactionHub.Rpc_Interact(_playerResolver.Id, _focusedInteractable.BehaviourId);
            _currentInteractable = _focusedInteractable;
            _castSensor.SetEnabled(false);
            
            UnFocus();
        }
        
        private void OnReleasePressed()
        {
            if (_currentInteractable == null) return;
            
            _interactionHub.Rpc_Release(_playerResolver.Id, _currentInteractable.BehaviourId);
            _currentInteractable = null;
            _castSensor.SetEnabled(true);
        }
    }
}