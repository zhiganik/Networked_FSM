using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Fusion;
using RootMotion.FinalIK;
using Shakhtarsk.Characters.IK;
using Shakhtarsk.Characters.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Shakhtarsk.Interactions
{
    [RequireComponent(typeof(InteractionObject))]
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableObject : Interactable
    {
        [TabGroup("References", TextColor = "green"), Required]
        [SerializeField] private InteractionObject interactionObject;
        
        [TabGroup("References", TextColor = "green"), Required]
        [SerializeField] private Rigidbody rigidBody;
        
        [TabGroup("References", TextColor = "green"), Required]
        [SerializeField] private IkHand[] ikHands;
        
        [TabGroup("References", TextColor = "green"), Required]
        [SerializeField] private Transform pivotPoint;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField] private AttachType attachType;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField] private bool canInterrupt = true;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField] private bool turnOffHandMesh;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField, Range(0.1f, 1f)] private float pickUpTime = 0.3f;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField, Range(0,1f)] private float fadeDuration;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField] private Vector3 remappedForward;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField] private Vector3 holdOffset;
        
        [TabGroup("Settings", TextColor = "white")]
        [SerializeField, Range(0, 1)] private float movementGrabDelay;
        
        [TabGroup("States", TextColor = "cyan"), GUIColor(0.1f, 0.7f, 1f)]
        [Networked, OnChangedRender(nameof(IsHoldChanged))] public NetworkBool HoldNetwork { get; private set; }
        
        [TabGroup("States", TextColor = "cyan"), GUIColor(0.1f, 0.7f, 1f)]
        [Networked, OnChangedRender(nameof(IsGrabbedChanged))] public NetworkBool GrabbedNetwork { get; private set; }
        
        [field: TabGroup("States", TextColor = "cyan"), GUIColor(1f, 1f, 1f)]
        [field: SerializeField] public bool IsHold { get; set; }
        
        [field: TabGroup("States", TextColor = "cyan"), GUIColor(1f, 1f, 1f)]
        [field: SerializeField] public bool IsGrabbed { get; set; }

        private Transform _holdPoint;
        private Collider _collider;
        private IkHolder _holder;
        private float _holdWeight;
        private float _holdWeightVelocity;
        private Vector3 _pickUpPosition;
        private Quaternion _pickUpRotation;
        private InteractionObject.InteractionEvent _pauseEvent;

        public override void Spawned()
        {
            _collider = GetComponent<Collider>();
            ValidateEvents();
            SubscribeOnPause();
        }

        private void OnValidate()
        {
            interactionObject ??= GetComponent<InteractionObject>();
            ikHands ??= GetComponentsInChildren<IkHand>();
            rigidBody ??= GetComponent<Rigidbody>();

            var networkTransform = GetComponent<NetworkTransform>();
            networkTransform.SyncParent = true;
            networkTransform.DisableSharedModeInterpolation = true;

            foreach (var ikHand in ikHands)
            {
                ikHand.SetHandsActive(!turnOffHandMesh);
            }
            
            ValidateEvents();
        }

        private void SubscribeOnPause()
        {
            _pauseEvent.unityEvent.AddListener(OnInteractionPaused);
        }

        private void OnInteractionPaused()
        {
            if (IsHold) return;
            
            rigidBody.isKinematic = true;
            transform.parent = _holder.transform;
            _pickUpPosition = transform.position;
            _pickUpRotation = transform.rotation;
            _holdWeight = 0f;
            _holdWeightVelocity = 0f;
            
            Rpc_SetNetworkHold(true);
            IsHold = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var rotation = Quaternion.LookRotation(remappedForward);
            var position = rotation * holdOffset;
            Gizmos.DrawSphere(transform.TransformPoint(position), 0.05f);
        }

        protected override async void OnInteract(PlayerResolver playerResolver)
        {
            if (IsGrabbed) return;
            
            var ikHolder = playerResolver.Resolve<IkHolder>();
            var player = playerResolver.Resolve<PlayerMechanism>();
            
            _holder = ikHolder;
            rigidBody.isKinematic = true;
            RotatePivot(ikHolder.IKInteraction);
            _holdPoint = ikHolder.GetPointByAttachType(attachType);
            _holdPoint.localPosition = holdOffset;
            _holdPoint.localRotation = GetRotationByDirection(_holdPoint.InverseTransformDirection(-_holdPoint.forward));

            foreach (var ikHand in ikHands)
            {
                var target = ikHand.InteractionTarget;
                ikHolder.IKInteraction.StartInteraction(target.effectorType, interactionObject, canInterrupt);
            }
            
            if (player.Kcc.RenderData.RealSpeed > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(movementGrabDelay));
                OnInteractionPaused();
            }

            _collider.enabled = false;
            IsGrabbed = true;
            Rpc_SetNetworkGrab(true);
        }

        protected override void OnRelease(PlayerResolver playerResolver)
        {
            var ikHolder = playerResolver.Resolve<IkHolder>();
            ikHolder.IKInteraction.ResumeAll();
            
            transform.parent = null;
            rigidBody.isKinematic = false;
            
            _collider.enabled = true;
            IsHold = false;
            IsGrabbed = false;

            Rpc_SetNetworkGrab(false);
            Rpc_SetNetworkHold(false);
        }

        public void LateUpdate()
        {
            if (Object == null || !IsGrabbed) return;
            
            if (IsHold)
            {
                _holdWeight = Mathf.SmoothDamp(_holdWeight, 1f, ref _holdWeightVelocity, pickUpTime);
                transform.position = Vector3.Lerp(_pickUpPosition, _holdPoint.position, _holdWeight);
                transform.rotation = Quaternion.Lerp(_pickUpRotation, _holdPoint.rotation, _holdWeight);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_SetNetworkGrab(NetworkBool isGrabbed)
        {
            GrabbedNetwork = isGrabbed;
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void Rpc_SetNetworkHold(NetworkBool isHold)
        {
            HoldNetwork = isHold;
        }

        private void IsGrabbedChanged() { }

        private void IsHoldChanged() { }
        
        protected virtual void RotatePivot(InteractionSystem interactionSystem)
        {
            var leftHand = interactionSystem.ik.solver.leftHandEffector.bone;
            var rightHand = interactionSystem.ik.solver.rightHandEffector.bone;
            var targetCenter = Vector3.Lerp(leftHand.position, rightHand.position, 0.5f);
            
            Vector3 dir = targetCenter - transform.position;
            dir.y = 0;

            transform.DORotate(GetRotationByDirection(dir).eulerAngles, fadeDuration);
        }
        
        private void ValidateEvents()
        {
            var events = interactionObject.events;
            
            if (events.Length == 0 || events.All(e => !e.pause))
            {
                throw new NullReferenceException("GrabbableObject must contain pause event");
            }

            _pauseEvent = events.First(e => e.pause);
        }

        private Quaternion GetRotationByDirection(Vector3 forward)
        {
            var remappedUp = new Vector3(0,1,0);
            var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));
            return Quaternion.LookRotation(forward) * axisRemapRotation;
        }
    }
}