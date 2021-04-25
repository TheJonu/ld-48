using UnityEngine;

namespace AI
{
    public class RandomMovement : Enemy
    {
        [SerializeField] private float maxTime = 2;
        [SerializeField] private float minTime = 5;
        [SerializeField] private bool flippable = true;

        private float _timeLeft = -1f;
        private int _direction = 0;
        
        
        private void FixedUpdate()
        {
            if (_timeLeft <= 0)
            {
                Stop();
                RollNew();
            }

            Vector2 targetSpeed = new Vector2(xSpeed * _direction, Rb2d.velocity.y);
            Vector2 ret = Vector2.zero;

            Vector2.SmoothDamp(Rb2d.velocity, targetSpeed, ref RefVel, SpeedDampening, xSpeed);

            Rb2d.velocity = RefVel;

            if (_direction == 0)
                Rb2d.velocity = Vector2.zero;

            _timeLeft -= Time.deltaTime;
        }

        private void RollNew()
        {
            int p = 0;
            int[] directionPick = new int[2];
            for(int i=-1; i<=1; i++)
            {
                if (i == _direction)
                    continue;
                directionPick[p++] = i;
            }
            _timeLeft = Random.Range(minTime, maxTime);
            _direction = directionPick[Random.Range(0, 2)];
        }
    }
}
