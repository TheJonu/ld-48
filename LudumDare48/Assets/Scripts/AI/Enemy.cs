using UnityEngine;

namespace AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float xSpeed = .5f;

        public EnemyCollisionHandle CollisionHandle { get; private set; }

        protected const float MovementThreshold = 0.1f;
        protected const float SpeedDampening = .05f;

        protected Rigidbody2D Rb2d;
        protected Vector2 RefVel = Vector2.zero;


        protected virtual void Start()
        {
            Rb2d = gameObject.GetComponent<Rigidbody2D>();
            CollisionHandle = gameObject.AddComponent<EnemyCollisionHandle>();
        }

        protected void Flip()
        {
            if (Rb2d.velocity.x > MovementThreshold)
            {
                Vector3 sc = gameObject.transform.localScale;
                sc.x = Mathf.Abs(sc.x);
                gameObject.transform.localScale = sc;
            }
            else if (Rb2d.velocity.x < -MovementThreshold)
            {
                Vector3 sc = gameObject.transform.localScale;
                sc.x = -Mathf.Abs(sc.x);
                gameObject.transform.localScale = sc;
            }
        }
        
        protected void Stop()
        {
            Vector2 targetSpeed = new Vector2(0, Rb2d.velocity.y);
            Vector2 ret = Vector2.zero;
            Rb2d.velocity = Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening);
        }
    }
}