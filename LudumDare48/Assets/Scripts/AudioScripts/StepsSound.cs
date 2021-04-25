using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioScripts
{
    public class StepsSound : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private AudioSource source;
        [SerializeField] private float volumeMod;
        [SerializeField] private float movementThreshold;
        [SerializeField] private float startTime;
        [SerializeField] private float stepTime;
        [SerializeField] private List<AudioClip> clips;
        
        private float _timer;
        private bool _isMoving;


        private void Start()
        {
            source.volume *= volumeMod;
        }

        private void Update()
        {
            _isMoving = rb.velocity.magnitude > movementThreshold;

            if (!_isMoving)
            {
                _timer = startTime;
                return;
            }
            
            _timer += Time.deltaTime;

            if (_timer > stepTime)
            {
                source.clip = clips.GetRandom();
                source.Play();
                
                _timer = 0;
            }
        }
    }
}