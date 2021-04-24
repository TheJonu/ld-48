using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Anim
{
    public class SpriteSheetMovementAnim : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float cycleTime;
        [SerializeField] private float movementSpeedModifier;
        [SerializeField] private float movementThreshold = 0.0015f;
        [SerializeField] private Sprite[] sprites;

        private float _timer;
        private Vector2 _prevPos;
        private Vector2 _posChange;
        private float _magnitude;


        private void FixedUpdate()
        {
            Calculate();
            Apply();
        }

        private void Calculate()
        {
            Vector2 pos = transform.position;
            _posChange = (pos - _prevPos) * Time.deltaTime;
            _magnitude = _posChange.magnitude;
            _prevPos = pos;
        }

        private void Apply()
        {
            if (_magnitude < movementThreshold)
            {
                sr.sprite = sprites[0];
            }
            else
            {
                _timer += Time.fixedDeltaTime + _magnitude * movementSpeedModifier;
                int number = Mathf.FloorToInt(_timer / (cycleTime / sprites.Length)) % sprites.Length;
                sr.sprite = sprites[number];
            }
        }
    }
}