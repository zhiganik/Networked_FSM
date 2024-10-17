using System;
using System.Collections.Generic;
using Shakhtarsk.Interactions;
using UnityEngine;

namespace Shakhtarsk.Sensors
{
    [RequireComponent(typeof(Collider))]
    public class TriggerSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private Collider collider;
        [SerializeField] private LayerMask mask;
        
        private readonly Dictionary<Collider, Component> _componentMap = new ();

        public bool Enable
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        public Type Filter { get; set; }
        public bool IsDetected { get; private set; }
        
        public event Action<SensorInfo> Detected = delegate { };
        public event Action<SensorInfo> Lost = delegate { };

        private void OnValidate()
        {
            collider ??= GetComponent<Collider>();
        }

        private void Awake()
        {
            collider.isTrigger = true;
            Filter = typeof(IInteractable);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!TryMutual(other, out var component)) return;
            
            IsDetected = true;

            _componentMap[other] = component;
            Detected.Invoke(new SensorInfo(component));
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_componentMap.Remove(other, out var lossComponent)) return;

            Lost.Invoke(new SensorInfo(lossComponent));
        }

        private bool TryMutual(Collider other, out Component component)
        {
            component = default;

            if (Filter != null)
                return (mask & (1 << other.gameObject.layer)) != 0 &&
                       other.TryGetComponent(Filter, out component);
            
            Debug.LogWarning($"Filter Type is not registered for {this}");
            return false;

        }
    }
}