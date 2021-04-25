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
            _timer += Time.fixedDeltaTime;
            
            Vector2 newPos = transform.position;
            _posChange = (newPos - _prevPos) * Time.deltaTime;
            _movementMagnitude = _posChange.magnitude;
            _prevPos = newPos;
            
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
                Debug.Log(playerController.LadderController);
                int number = Mathf.FloorToInt(_timer / spriteTime) % ladderSprites.Count;
                sr.sprite = ladderSprites[number];
            }
        }
    }
}