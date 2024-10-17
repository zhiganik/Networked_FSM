using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shakhtarsk.Sensors
{
    public abstract class CastSensor : ISensor
    {
        protected PhysicsScene physicsScene;
        
        private RaycastHit _hitInfo;
        private Component _detectedComponent;
        private bool _detected;

        private readonly Transform _transform;
        private readonly IDisposable _update;
        
        public bool Enable { get; private set; } = true;
        public Type Filter { get; set; }
        public bool IsDetected => _detected;
        public float MaxDistance { get; set; }
        public LayerMask LayerMask { get; set; } = ~0;
        public Vector3 LocalDirection { get; set; } = Vector3.zero;
        public Vector3 LocalOffset { get; set; } = Vector3.zero;
        
        public event Action<SensorInfo> Detected = delegate { };
        public event Action<SensorInfo> Lost = delegate { };

        public CastSensor(Transform transform, PhysicsScene? scene = null, FrameProvider frameProvider = null)
        {
            frameProvider ??= UnityFrameProvider.PostLateUpdate;
            physicsScene = scene ?? Physics.defaultPhysicsScene;
            
            _transform = transform;
            
            _update = Observable
                .EveryUpdate(frameProvider)
                .Where(_ => Enable)
                .Subscribe(_ => Update());
        }

        ~CastSensor()
        {
            _update.Dispose();
        }

        public void SetEnabled(bool state)
        {
            Enable = state;

            if (!state)
            {
                _detectedComponent = null;
            }
        }
        
        public abstract bool ThrowCast(ThrowArgs args, out RaycastHit hitInfo);

        public virtual void DrawGizmos()
        {
            if (_hitInfo.point == Vector3.zero) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_hitInfo.point, 0.2f);
        }
        

        protected Vector3 GetOrigin() => GetOrigin(LocalOffset);
        protected Vector3 GetOrigin(Vector3 localOffset)
        {
            return  _transform.TransformPoint(localOffset);
        }

        protected Vector3 GetDirection() => GetDirection(LocalDirection);
        protected Vector3 GetDirection(Vector3 localDirection)
        {
            return _transform.TransformDirection(localDirection);
        }

        private void Update()
        {
            var args = new ThrowArgs
            {
                origin = GetOrigin(),
                direction = GetDirection(),
                maxDistance = MaxDistance,
                mask = LayerMask
            };
            
            _detected = ThrowCast(args, out _hitInfo);
            if (_detected && TryMutual(_hitInfo.collider, out var component))
            {
                if (component == _detectedComponent) return;
                
                TryLost();
                _detectedComponent = component;
                Detected?.Invoke(new SensorInfo(_detectedComponent));
            }
            else TryLost();
        }

        private bool TryMutual(Collider other, out Component component)
        {
            component = default;

            if (Filter != null) return other && other.TryGetComponent(Filter, out component);
            
            Debug.LogWarning($"Filter Type is not registered for {this}");
            return false;
        }

        private void TryLost()
        {
            if (_detectedComponent == null) return;
            
            var lossComponent = _detectedComponent;
            _detectedComponent = null;
                
            Lost.Invoke(new SensorInfo(lossComponent));
        }
    }
}