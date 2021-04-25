using System;
using System.Collections.Generic;
using Anim;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioScripts
{
    public class StepsSound : MonoBehaviour
    {
        [SerializeField] private SpriteSheetAnim spriteSheetAnim;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float startTime;
        [SerializeField] private float stepTime;
        [SerializeField] private List<AudioClip> clips;

        private float _timer;

        
        private void Update()
        {
            if (spriteSheetAnim.State != SpriteSheetAnim.AnimState.Movement)
            {
                _timer = startTime;
                return;
            }
            
            _timer += Time.deltaTime;

            if (_timer > stepTime)
            {
                audioSource.clip = clips.GetRandom();
                audioSource.Play();
                
                _timer = 0;
            }
        }
    }
}