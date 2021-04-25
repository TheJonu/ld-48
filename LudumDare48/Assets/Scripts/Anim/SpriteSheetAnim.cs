using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Anim
{
    public class SpriteSheetAnim : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float spriteTime;
        [SerializeField] private List<Sprite> idleSprites;      // must have at least one idle sprite
        [SerializeField] private List<Sprite> movementSprites;
        [SerializeField] private List<Sprite> ladderSprites;

        private const float MovementThreshold = 0.001f;
        
        private float _timer;
        private float _movement;

        private AnimState _state = AnimState.Idle;
        
        private bool _hasMovementSprites;
        private bool _hasLadderSprites;
        private bool _hasPlayerController;

        
        private enum AnimState { Idle, Movement, Ladder}
        
        
        private void Start()
        {
            _hasMovementSprites = !(movementSprites is null) && movementSprites.Count != 0;
            _hasLadderSprites = !(ladderSprites is null) && ladderSprites.Count != 0;
            _hasPlayerController = !(playerController is null);
        }

        private void Update()
        {
            _movement = rb.velocity.magnitude;
            _timer += Time.deltaTime;

            CheckState();
            Apply();
        }

        private void CheckState()
        {
            AnimState newState = AnimState.Idle;
            
            if (_hasMovementSprites && _movement > MovementThreshold)
                newState = AnimState.Movement;
            if (_hasLadderSprites && _hasPlayerController && !(playerController.LadderController is null))
                newState = AnimState.Ladder;

            if (newState != _state)
                _timer = 0;

            _state = newState;
        }

        private void Apply()
        {
            List<Sprite> sprites = _state switch
            {
                AnimState.Movement => movementSprites,
                AnimState.Ladder => ladderSprites,
                _ => idleSprites
            };
            
            int number = Mathf.FloorToInt(_timer / spriteTime) % sprites.Count;
            sr.sprite = sprites[number];
        }
    }
}