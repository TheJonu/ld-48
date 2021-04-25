using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Anim
{
    public class SpriteSheetAnim : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float spriteTime;
        [SerializeField] private float movementThreshold = 0.0015f;
        [SerializeField] private List<Sprite> idleSprites;
        [SerializeField] private List<Sprite> movementSprites;
        [SerializeField] private List<Sprite> ladderSprites;

        private float _timer;
        private Vector2 _prevPos;
        private Vector2 _posChange;
        private float _movementMagnitude;

        private bool _hasIdleSprites;
        private bool _hasMovementSprites;
        private bool _hasLadderSprites;
        private bool _hasPlayerController;

        
        private void Start()
        {
            _hasIdleSprites = !(idleSprites is null) && idleSprites.Count != 0;
            _hasMovementSprites = !(movementSprites is null) && movementSprites.Count != 0;
            _hasLadderSprites = !(ladderSprites is null) && ladderSprites.Count != 0;
            _hasPlayerController = !(playerController is null);
        }

        private void FixedUpdate()
        {
            Vector2 newPos = transform.position;
            _posChange = (newPos - _prevPos) * Time.fixedDeltaTime;
            _movementMagnitude = _posChange.magnitude;
            _prevPos = newPos;

            if (_posChange.x != 0.0f || _posChange.y != 0.0f)
            {
                _timer += Time.fixedDeltaTime;
            }
            // Make the movement anim always start from the same point
            else if(playerController.LadderController is null)
            {
                _timer = 0.0f;
            }


            Apply();
        }

        private void Apply()
        {
            if (_hasIdleSprites && _movementMagnitude <= movementThreshold)
            {
                int number = Mathf.FloorToInt(_timer / spriteTime) % idleSprites.Count;
                sr.sprite = idleSprites[number];
            }
            
            if (_hasMovementSprites && _movementMagnitude > movementThreshold)
            {
                int number = Mathf.FloorToInt(_timer / spriteTime) % movementSprites.Count;
                sr.sprite = movementSprites[number];
            }
            
            if (_hasLadderSprites && _hasPlayerController && !(playerController.LadderController is null))
            {
                int number = Mathf.FloorToInt(_timer / spriteTime) % ladderSprites.Count;
                sr.sprite = ladderSprites[number];
            }
        }
    }
}