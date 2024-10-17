using UnityEngine;

namespace Shakhtarsk.Sensors
{
    public struct ThrowArgs
    {
        public Vector3 origin;
        public Vector3 direction;
        public float maxDistance;
        public LayerMask mask;
    }
}