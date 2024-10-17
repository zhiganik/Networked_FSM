using Fusion;
using UnityEngine;

namespace Shakhtarsk.Develop
{
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetworkTransform))]
    public class MovingPlatform : NetworkBehaviour
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private float delayAtPoint = 1f;
        
        private bool _isInverse;
        private float _waitTimer = 0f;
        private bool _isWaiting;
        private Vector3 _platformPosition;

        public override void Spawned()
        {
            if(!HasStateAuthority) return;
            
            _isWaiting = true;
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasStateAuthority) return;

            if (!_isWaiting)
            {
                MovePlatform();
            }
            else
            {
                HandleWaiting();
            }

            transform.localPosition =
                new Vector3(transform.localPosition.x, _platformPosition.y, transform.localPosition.z);
        }

        private void MovePlatform()
        {
            Vector3 targetPosition = !_isInverse ? endPoint.localPosition : startPoint.localPosition;
            float step = speed * Runner.DeltaTime;
            _platformPosition = Vector3.MoveTowards(_platformPosition, targetPosition, step);
            
            if (Vector3.Distance(_platformPosition, targetPosition) < 0.002f)
            {
                _isWaiting = true;
                _waitTimer = delayAtPoint;
                _isInverse = !_isInverse;
            }
        }
        
        private void HandleWaiting()
        {
            _waitTimer -= Runner.DeltaTime;

            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
            }
        }
    }
}