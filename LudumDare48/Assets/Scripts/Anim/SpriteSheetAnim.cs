using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEngine;

namespace Anim
{
    public class SpriteSheetAnim : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float spriteTime;
        [SerializeField] private bool onlyHorizontalMovement;
        [SerializeField] private List<Sprite> idleSprites;      // must have at least one idle sprite
        [SerializeField] private List<Sprite> movementSprites;
        [SerializeField] private List<Sprite> ladderSprites;

        public AnimState State { get; set; } = AnimState.Idle;
        
        private const float MovementThreshold = 0.001f;
        private const float MinHorizontalVerticalMovementRatio = 1f/3f;

        private float _timer;
        private Vector2 _movement;


        private bool _hasMovementSprites;
        private bool _hasLadderSprites;
        private bool _hasPlayerController;

        
        public enum AnimState { Idle, Movement, Ladder}
        
        
        private void Start()
        {
            _hasMovementSprites = !(movementSprites is null) && movementSprites.Count != 0;
            _hasLadderSprites = !(ladderSprites is null) && ladderSprites.Count != 0;
            _hasPlayerController = !(playerController is null);
        }

        private void Update()
        {
            _movement = rb.velocity;
            _timer += Time.deltaTime;

            CheckState();
            Apply();
        }

        private void CheckState()
        {
            AnimState newState = AnimState.Idle;

            float horizontalVerticalRatio = Mathf.Abs(_movement.x) / Mathf.Abs(_movement.y);
            
            if (onlyHorizontalMovement && _hasMovementSprites && Mathf.Abs(_movement.x) > MovementThreshold && horizontalVerticalRatio > MinHorizontalVerticalMovementRatio)
                newState = AnimState.Movement;
            
            if (!onlyHorizontalMovement && _hasMovementSprites && _movement.magnitude > MovementThreshold)
                newState = AnimState.Movement;

            if (_hasLadderSprites && _hasPlayerController && !(playerController.LadderController is null))
            {
                newState = AnimState.Ladder;
                if (!playerController.LadderController.IsMoving)
                {
                    _timer = 0;
                }
            }
                

            if (newState != State)
                _timer = 0;

            State = newState;
        }

        private void Apply()
        {
            List<Sprite> sprites = State switch
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