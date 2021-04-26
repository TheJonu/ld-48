using UnityEngine;

namespace AI
{
    public class ChaserEnemy : Enemy
    {
        [SerializeField] private Transform toChase;
        [SerializeField] private float activationDistance;
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
            
            if (Vector2.Distance(toChase.position, transform.position) >= activationDistance)
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