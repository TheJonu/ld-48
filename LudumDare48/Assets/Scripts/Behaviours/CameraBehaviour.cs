using UnityEngine;

namespace Behaviours
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private Rigidbody2D followRb;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float aimDist;

        private Vector2 _aimPos;

        private void Start()
        {
            if (follow)
            {
                transform.position = (Vector2)follow.position;
            }
        }

        private void FixedUpdate()
        {
            if (!follow)
                return;
            
            _aimPos = follow.position;

            if (followRb)
                _aimPos += aimDist * followRb.velocity;
            
            transform.position = Vector2.Lerp(transform.position, _aimPos, moveSpeed * Time.fixedDeltaTime);
        }
    }
}