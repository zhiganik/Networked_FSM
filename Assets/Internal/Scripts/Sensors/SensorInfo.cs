using UnityEngine;

namespace Shakhtarsk.Sensors
{
    public struct SensorInfo
    {
        private readonly Component _component;

        public SensorInfo(Component component)
        {
            _component = component;
        }

        public bool TryGetComponent<T>(out T outComponent)
        {
            outComponent = default;
            if (_component is not T tempComponent) return false;
            
            outComponent = tempComponent;
            return true;
        }
    }
}