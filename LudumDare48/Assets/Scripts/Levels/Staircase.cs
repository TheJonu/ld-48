using System;
using UnityEngine;

namespace Levels
{
    public class Staircase : LevelBlock
    {
        [SerializeField] private Transform entrance;
        [SerializeField] private Transform exit;

        
        public Vector2 CenterPosition => transform.position;
        public Vector2 EntranceLocalPos => entrance.localPosition;
        public Vector2 ExitLocalPos => exit.localPosition;
        public Vector2 EntrancePos => entrance.position;
        public Vector2 ExitPos => exit.position;
        
        
        public Action StaircaseEntered;


        private void OnTriggerEnter(Collider other)
        {
            // TODO check if player
            StaircaseEntered?.Invoke();
        }
    }
}