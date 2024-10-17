using System.Collections.Generic;
using Fusion;
using Fusion.Addons.FSM;
using Fusion.Addons.KCC;
using RootMotion.FinalIK;
using Shakhtarsk.Characters.Player.Data;
using Shakhtarsk.Characters.Player.FSM;
using Shakhtarsk.Characters.Player.FSM.Data;
using Shakhtarsk.Characters.Player.Network;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shakhtarsk.Characters.Player
{
    [RequireComponent(typeof(StateMachineController))]
    public class PlayerMechanism : NetworkBehaviour, IPlayerComponent, IStateMachineOwner
    {
        [SerializeField] private KCC kcc;
        [SerializeField] private Animator animator;
        [SerializeField] private KCCStatesData statesData;
        [SerializeField] private FullBodyBipedIK ik;
        [SerializeField] private List<PlayerStateBehaviour> behaviours;

        [Header("Settings"), Space]
        [SerializeField, Range(0,10f)] private float mouseSensitivity;
        [SerializeField, Range(0,5f)] private float moveSpeed = 2;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0,0.1f,0.15f);
        
        [Networked] public NetworkButtons PreviousButtons { get; set; }
        [Networked] public Vector2 CurrentInput { get; set; }
        [Networked] public FallInstruction CurrentFallInstruction { get; set; }
        [Networked, OnChangedRender(nameof(HandleLock))] public NetworkBool IsLocked { get; set; }
        
        private Camera _targetCamera;
        private PlayerBehaviourMachine _stateMachine;
        private FallInstruction _defaultFallInstruction;
        private EnvironmentProcessor _environmentProcessor;

        public KCC Kcc => kcc;

        private bool _sprintWasPressed;
        private float _movementSpeed;
        private bool _jumpWasPressed;
        
        public bool IsJump => _jumpWasPressed;
        public bool IsSprint => kcc.Data.Sprint;
        public bool IsCrouch => kcc.Data.Crouch;
        
        public void CollectStateMachines(List<IStateMachine> stateMachines)
        {
            _stateMachine = new PlayerBehaviourMachine("Player", this, animator, statesData, behaviours.ToArray());
            stateMachines.Add(_stateMachine);
        }

        public override void Spawned()
        {
            _environmentProcessor = kcc.GetProcessor<EnvironmentProcessor>(); 
            HandleLock();

            if(!HasStateAuthority) return;
            
            CurrentFallInstruction = new FallInstruction(true);
        }

        public void SetCamera(Camera targetCamera)
        {
            if (HasStateAuthority && HasInputAuthority)
            {
                _targetCamera = targetCamera;
                _targetCamera.transform.parent = ik.solver.headMapping.bone;
                _targetCamera.transform.localPosition = cameraOffset;
            }
        }
        
        public void SetCharacterHeight(float height)
        {
            kcc.SetHeight(height);
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput<NetworkPlayerInput>(out var playerInput) || !Runner.ProvideInput) return;
            
            foreach (var playerState in behaviours)
            {
                _stateMachine.TryActivateState(playerState);
            }
            
            var pressed = playerInput.Buttons.GetPressed(PreviousButtons);
            var released = playerInput.Buttons.GetReleased(PreviousButtons);

            PreviousButtons = playerInput.Buttons;
            CurrentInput = playerInput.Input;

            kcc.AddLookRotation(playerInput.Mouse * Runner.DeltaTime * mouseSensitivity);

            if (!IsLocked)
            {
                Vector3 inputDirection = kcc.Data.TransformRotation * new Vector3(CurrentInput.x, 0.0f, CurrentInput.y);
                kcc.SetInputDirection(inputDirection);
            }
          
            ProceedButtons(pressed, released);
        }
        
        private void HandleLock()
        {
            _environmentProcessor.KinematicSpeed = IsLocked ? 0 : moveSpeed;
        }

        private void ProceedButtons(NetworkButtons pressed, NetworkButtons released)
        {
            if (!kcc.RenderData.IsGrounded) return;
            
            _jumpWasPressed = pressed.IsSet(NetworkButtonsFlags.Jump);

            if (!_jumpWasPressed && pressed.IsSet(NetworkButtonsFlags.Crouch))
            {
                var crouch = !kcc.Data.Crouch;
                kcc.SetCrouch(crouch);
            }

            if (IsCrouch) return;
            
            if (pressed.IsSet(NetworkButtonsFlags.Sprint)) kcc.SetSprint(true);

            if (released.IsSet(NetworkButtonsFlags.Sprint)) kcc.SetSprint(false);
        }

        private void LateUpdate()
        {
            if (Object.IsProxy || !Runner.ProvideInput) return;
            
            Vector2 pitchRotation = kcc.GetLookRotation();
            _targetCamera.transform.rotation = Quaternion.Euler(pitchRotation);
        }
    }
}