using UnityEngine;

namespace AI
{
    public class RandomMoveEnemy : Enemy
    {
        [SerializeField] private float maxTime = 2;
        [SerializeField] private float minTime = 5;
        [SerializeField] private bool flippable = true;

        [SerializeField] private Transform leftBound;
        [SerializeField] private Transform rightBound;

        private float _timeLeft = -1f;

        private Vector3 _location;

        private void Start()
        {
            Rb2d = GetComponent<Rigidbody2D>();
            RollNewLoc();
        }

        private void FixedUpdate()
        {
            if(_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                return;
            }

            else if(Vector3.Distance(_location, transform.position) <= .1f)
            {
                Stop();
                RollNew();
                RollNewLoc();
                return;
            }

            Vector2.SmoothDamp(transform.position, _location, ref RefVel, SpeedDampening, xSpeed);

            Rb2d.velocity = RefVel;
            
            if(flippable)
                Flip();
        }

        private void RollNew()
        {
            _timeLeft = Random.Range(minTime, maxTime);
        }

        private void RollNewLoc()
        {
            float locx = Random.Range(leftBound.position.x, rightBound.position.x);
            float locy = Random.Range(leftBound.position.y, rightBound.position.y);
            float locz = transform.position.z;

            _location = new Vector3(locx, locy, locz);
        }
    }
}
