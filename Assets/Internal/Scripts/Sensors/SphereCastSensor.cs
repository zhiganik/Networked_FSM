using R3;
using UnityEngine;

namespace Shakhtarsk.Sensors
{
    public class SphereCastSensor : CastSensor
    {
        public float SphereCastRadius { get; }
        
        public SphereCastSensor(Transform transform, float sphereCastRadius, PhysicsScene? physicsScene, FrameProvider frameProvider = null) 
            : base(transform, physicsScene, frameProvider)
        {
            SphereCastRadius = sphereCastRadius;
        }

        public override bool ThrowCast(ThrowArgs args, out RaycastHit hitInfo)
        {
            Debug.DrawRay(args.origin, args.direction * args.maxDistance);
            return physicsScene.SphereCast(args.origin, SphereCastRadius, args.direction, out hitInfo, args.maxDistance,
                args.mask);
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.green;

            var upOffset = new Vector3(0, SphereCastRadius, 0);
            var downOffset = new Vector3(0, -SphereCastRadius, 0);
            
            var leftOffset = new Vector3(-SphereCastRadius, 0, 0);
            var rightOffset = new Vector3(SphereCastRadius, 0, 0);
            
            Gizmos.DrawRay(GetOrigin(upOffset), GetDirection(LocalDirection) * MaxDistance);
            Gizmos.DrawRay(GetOrigin(downOffset), GetDirection(LocalDirection) * MaxDistance);
            Gizmos.DrawRay(GetOrigin(leftOffset), GetDirection(LocalDirection) * MaxDistance);
            Gizmos.DrawRay(GetOrigin(rightOffset), GetDirection(LocalDirection) * MaxDistance);
            Gizmos.DrawWireSphere(GetOrigin() + GetDirection() * MaxDistance, SphereCastRadius);
        }
    }
}