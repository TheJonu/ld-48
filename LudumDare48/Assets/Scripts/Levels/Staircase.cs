using System;
using UnityEngine;

namespace Levels
{
    public class Staircase : MonoBehaviour
    {
        [SerializeField] private Transform entrance;
        [SerializeField] private Transform exit;

        public Vector2 EntranceLocalPos => entrance.localPosition;
        public Vector2 ExitPos => exit.position;

        public Action StaircaseEntered;


        private void OnTriggerEnter(Collider other)
        {
            // TODO check if player
            StaircaseEntered?.Invoke();
        }
    }
}