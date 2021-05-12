using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class ChaserEnemy : Enemy
    {
        [SerializeField] private Transform toChase;
        [FormerlySerializedAs("activationDistance")] [SerializeField] private float xActivationDistance;
        [SerializeField] private float yActivationDistance;
        [SerializeField] private bool chasePlayer;

        public Transform ToChase { set => toChase = value; }

        protected override void Start()
        {
            base.Start();
            if (chasePlayer)
                toChase = GameObject.FindWithTag("Player").transform;
        }

        private void FixedUpdate()
        {
            if(!toChase)
                return;
            
            if (Mathf.Abs(toChase.position.x - transform.position.x) >= xActivationDistance || Mathf.Abs(toChase.position.y - transform.position.y) >= yActivationDistance)
            {
                Vector2 targetSpeed = new Vector2(0, Rb2d.velocity.y);
                Vector2 ret = Vector2.zero;
                Rb2d.velocity = Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening);
            }

            else if (transform.position.x < toChase.position.x)
            {
                Vector2 targetSpeed = new Vector2(xSpeed, Rb2d.velocity.y);
                Vector2 ret = Vector2.zero;
                Rb2d.velocity = Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening);
            }

            else if (transform.position.x > toChase.position.x)
            {
                Vector2 targetSpeed = new Vector2(-xSpeed, Rb2d.velocity.y);
                Vector2 ret = Vector2.zero;
                Rb2d.velocity = Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening);
            }
            
            else
            {
                Vector2 targetSpeed = new Vector2(0, Rb2d.velocity.y);
                Vector2 ret = Vector2.zero;
                Rb2d.velocity = Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening);
            }

            Flip();
        }
    }
}
