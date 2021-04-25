using UnityEngine;

namespace Behaviours
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform follow;
        [SerializeField] private Rigidbody2D followRb;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 aimDist;

        private float CurrentLevelPosY
        {
            get
            {
                if (LevelManager.Instance.CurrentLevel is null)
                    return follow.position.y;
                else
                    return LevelManager.Instance.CurrentLevel.Entrance.ExitPos.y + 2.5f;
            }
        }

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

            if (Mathf.Abs(follow.position.y - CurrentLevelPosY) < 4f)
                _aimPos = new Vector2(follow.position.x, Mathf.Lerp(follow.position.y, CurrentLevelPosY, 0.5f));
            else
                _aimPos = follow.position;
            
            if (followRb)
                _aimPos += new Vector2(aimDist.x * followRb.velocity.x, aimDist.y * followRb.velocity.y);
            
            transform.position = Vector2.Lerp(transform.position, _aimPos, moveSpeed * Time.fixedDeltaTime);
        }
    }
}