using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Anim
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Light2D lightSource;
        [SerializeField] private float changeSpeed;
        //[SerializeField] private float minIntensity;
        [SerializeField] private Vector2 colorHueBounds;
        [SerializeField] private Vector2 colorSaturationBounds;
        [SerializeField] private Vector2 colorValueBounds;

        private float _timer;

        //private float _origIntensity;
        
        //private float _intensityNoiseY;
        private float _hueNoiseY;
        private float _saturationNoiseY;
        private float _valueNoiseY;
        
        private void Start()
        {
            //_origIntensity = lightSource.intensity;

            _hueNoiseY = Random.Range(1f, 10f);
            _saturationNoiseY = Random.Range(1f, 10f);
            _valueNoiseY = Random.Range(1f, 10f);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            
            //lightSource.intensity = minIntensity + (_origIntensity - minIntensity) * Mathf.PerlinNoise(changeSpeed * _timer, _intensityNoiseY);
            
            float hue = Mathf.Lerp(colorHueBounds.x, colorHueBounds.y, Mathf.PerlinNoise(changeSpeed * _timer, _hueNoiseY));
            float saturation = Mathf.Lerp(colorSaturationBounds.x, colorSaturationBounds.y, Mathf.PerlinNoise(changeSpeed * _timer, _saturationNoiseY));
            float value = Mathf.Lerp(colorValueBounds.x, colorValueBounds.y, Mathf.PerlinNoise(changeSpeed * _timer, _valueNoiseY));
            
            lightSource.color = Color.HSVToRGB(hue / 255f, saturation / 100f, value / 100f);
        }
    }
}