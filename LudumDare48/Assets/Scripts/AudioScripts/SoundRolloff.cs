using System;
using UnityEngine;

namespace AudioScripts
{
    public class SoundRolloff : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float maxDist;

        private float _origVolume;
        private Transform _listener;

        private void Start()
        {
            _origVolume = audioSource.volume;
            _listener = Camera.main.transform;
        }

        private void Update()
        {
            audioSource.volume = Mathf.Lerp(_origVolume, 0f, Vector2.Distance(_listener.position, transform.position) / maxDist);
        }
    }
}