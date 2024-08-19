using UnityEngine;

namespace __Project__.Scripts.Sensors
{
    public class RaySensor
    {
        private float _castLength;
        private LayerMask _layerMask;
        private readonly Transform _targetTransform;

        private Vector3 _origin = Vector3.zero;
        private CastDirection _castDirection;
        private RaycastHit _hitInfo;

        public Vector3 OriginPoint => _origin;

        public RaySensor(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        public  void Cast()
        {
            Vector3 worldOrigin = _targetTransform.TransformPoint(_origin);
            Vector3 worldDirection = GetCastDirection();

            Physics.Raycast(worldOrigin, worldDirection, out _hitInfo, _castLength);
        }
        
        public void SetCastLength(float castLength) => _castLength = castLength;
        public void SetLayerMask(LayerMask layerMask) => _layerMask = layerMask;
        public void SetCastDirection(CastDirection direction) => _castDirection = direction;
        public void SetCastOrigin(Vector3 pos) => _origin = _targetTransform.InverseTransformPoint(pos);
        
        public bool HasDetectedHit() => _hitInfo.collider != null;
        public float GetDistance() => _hitInfo.distance;
        public Vector3 GetNormal() => _hitInfo.normal;
        public Vector3 GetPosition() => _hitInfo.point;
        public Collider GetCollider() => _hitInfo.collider;
        public Transform GetTransform() => _hitInfo.transform;
        
        private Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Forward => _targetTransform.forward,
                CastDirection.Right => _targetTransform.right,
                CastDirection.Up => _targetTransform.up,
                CastDirection.Backward => -_targetTransform.forward,
                CastDirection.Left => -_targetTransform.right,
                CastDirection.Down => -_targetTransform.up,
                _ => Vector3.one
            };
        }
    }
}