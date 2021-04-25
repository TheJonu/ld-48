using UnityEngine;

namespace AI
{
    public class PatrollerEnemy : Enemy
    {
        [SerializeField] private Transform goesFrom;
        [SerializeField] private Transform goesTo;
        [SerializeField] private bool flippable = true;
        

        private void FixedUpdate()
        {
            if (!goesFrom || !goesTo)
                return;
            
            if (Vector2.Distance(goesTo.position, transform.position) <= MovementThreshold) 
            {
                Transform swap = goesTo;
                goesTo = goesFrom;
                goesFrom = swap;
                Stop();
            }

            Vector2.SmoothDamp(Rb2d.position, goesTo.position, ref RefVel, SpeedDampening, xSpeed);

            Rb2d.velocity = RefVel;

            if(flippable)
                Flip();
        }
    }
}
