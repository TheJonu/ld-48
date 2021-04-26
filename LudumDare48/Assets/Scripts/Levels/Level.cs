using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform floorParent;
        [SerializeField] private Transform ceilingParent;
        [SerializeField] private Transform backgroundParent;
        [SerializeField] private Transform objectsParent;
        //[SerializeField] private BoxCollider2D floorBoxCollider;
        [SerializeField] private CollisionHandler boxCollisionHandler;

        public LevelDataSet Data { get; set; }
        public Staircase Entrance { get; set; }
        public Staircase Exit { get; set; }

        public List<LevelBlock> LevelBlocks => _blocks;     // blocks - terrain tiles, staircases (with terrain as children)
        public List<LevelObject> LevelObjects => _objects;  // objects - furniture, sublevels (with terrain as children)
        public List<GameObject> GetSprites()         // all sprite renderers on the level
        {
            List<GameObject> list = new List<GameObject>();
            foreach (var block in _blocks)
            {
                if (block.TryGetComponent(typeof(SpriteRenderer), out var sr))
                    list.Add(sr.gameObject);
                foreach (Transform child in block.transform)
                    if (child.TryGetComponent(typeof(SpriteRenderer), out var childSr))
                        list.Add(childSr.gameObject);
            }
            foreach (var obj in _objects)
            {
                if (obj.TryGetComponent(typeof(SpriteRenderer), out var sr))
                    list.Add(sr.gameObject);
                foreach (Transform child in obj.transform)
                    if (child.TryGetComponent(typeof(SpriteRenderer), out var childSr))
                        list.Add(childSr.gameObject);
            }
            return list;
        }

        private float FloorSurfaceY => Entrance.ExitPos.y + 0.5f;
        private float CeilingSurfaceY => Entrance.EntrancePos.y + Data.CeilingYOffset - 0.5f;
        private float GetBottomBoundary(LevelObject prefab) => FloorSurfaceY + prefab.Dim.y / 2;
        private float GetTopBoundary(LevelObject prefab) => CeilingSurfaceY - prefab.Dim.y / 2;
        private float GetLeftBoundaryX(LevelObject prefab) => Mathf.RoundToInt(Entrance.ExitPos.x) + Data.Direction * (ObjectsBufferDist + prefab.Dim.x / 2);
        private float GetRightBoundaryX(LevelObject prefab) => Mathf.RoundToInt(Exit.EntrancePos.x) - Data.Direction * (ObjectsBufferDist + prefab.Dim.x / 2);
        private float SnapToGrid(float value) => value - value % Grid;

        private const int ObjectsBufferDist = 2;            // how far from staircases can objects be spawned
        private const int SpawnTries = 100;                 // how many times should a random position be genarated before giving up
        private const float CollCheckSizeScale = 0.95f;     // multiplier for the size of an object's effective collider
        private const float FloorColliderScaleY = 1.01f;    // scale multiplier for floor collider (should be a bit greater than 1)
        private const float Grid = 0.5f;                    // distance between elements on the grid

        private LayerMask _objectsMask;
        private LayerMask _terrainMask;
        private LayerMask _ladderMask;
        private LayerMask _anyCollMask;
        
        private List<LevelBlock> _blocks;
        private List<LevelObject> _objects;


        public void GenerateLevel()
        {
            _objectsMask = LayerMask.GetMask("Object");
            _terrainMask = LayerMask.GetMask("Terrain");
            _ladderMask = LayerMask.GetMask("Ladder");
            _anyCollMask = _objectsMask + _terrainMask + _ladderMask;

            _blocks = new List<LevelBlock>();
            _objects = new List<LevelObject>();

            GenerateExit();

            GenerateFloor();
            GenerateCeiling();
            GenerateBackground();

            SpawnObjects();
        }
        
        private void GenerateExit()
        {
            Vector2 exitPos = Entrance.ExitPos + Data.Length * Data.FloorPrefab.Dim.x * Data.Direction * Vector2.right;
            Exit = Instantiate(Data.StaircasePrefab, exitPos, Quaternion.identity, LevelManager.Instance.LevelsParent);
            Exit.transform.Translate(-Exit.EntranceLocalPos);
            Exit.transform.Translate(Data.FloorPrefab.Dim.x * Data.Direction * Vector2.left);
        }

        private void GenerateFloor()
        {
            Vector2 startPos = Entrance.ExitPos + new Vector2(-Data.Direction * Entrance.Dim.x, Data.FloorYOffset);
            Vector2 blockDist = Data.FloorPrefab.Dim.x * Data.Direction * Vector2.right;
            int length = Data.Length + Mathf.RoundToInt(Entrance.Dim.x);
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(Data.FloorPrefab, startPos + i * blockDist, Quaternion.identity, floorParent);
                _blocks.Add(block);
            }

            Vector2 centerDist = (length - 1) * blockDist.x / 2 * Vector2.right;
            boxCollisionHandler.Collider.transform.position = startPos + centerDist;
            boxCollisionHandler.Collider.size = new Vector2(length * Data.FloorPrefab.Dim.x, Data.FloorPrefab.Dim.y * FloorColliderScaleY);
            boxCollisionHandler.EnteredCollision += () => LevelManager.Instance.CurrentLevel = this;
        }

        private void GenerateCeiling()
        {
            Vector2 startPos = new Vector2(Entrance.ExitPos.x, Entrance.EntrancePos.y + Data.CeilingYOffset);
            Vector2 blockDist = Data.CeilingPrefab.Dim.x * Data.Direction * Vector2.right;
            int length = Data.Length + Mathf.RoundToInt(Exit.Dim.x);
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(Data.CeilingPrefab, startPos + i * blockDist, Quaternion.identity,
                    ceilingParent);
                _blocks.Add(block);
            }
        }

        private void GenerateBackground()
        {
            float y = Entrance.ExitPos.y;
            float startPosX = Entrance.ExitPos.x - Data.Direction * Entrance.Dim.x;
            float endPosX = Exit.EntrancePos.x + Data.Direction * Exit.Dim.x;
            int length = Mathf.FloorToInt(Mathf.Abs(endPosX - startPosX) / Data.BackgroundPrefab.Dim.x) + 1;
            if (Data.BackgroundPrefab.Dim.x > 1) length--;
            for (int i = 0; i < length; i++)
            {
                float x = startPosX + i * Data.Direction * Data.BackgroundPrefab.Dim.x;
                var block = Instantiate(Data.BackgroundPrefab, new Vector2(x, y), Quaternion.identity,
                    backgroundParent);
                _blocks.Add(block);
            }
        }

        private void SpawnObjects()
        {
            foreach (var spawn in Data.LevelObjects)
            {
                for (int i = 0; i < spawn.amount; i++)
                {
                    Func<LevelObject, LevelObject> generator = spawn.prefab.PositionVariant switch
                    {
                        PositionVariant.Floor => SpawnFloorStandingObject,
                        PositionVariant.Ceiling => SpawnCeilingHangingObject,
                        PositionVariant.Wall => SpawnWallHangingObject,
                        _ => o => null
                    };
                    var lo = generator.Invoke(spawn.prefab);
                    if(lo != null) _objects.Add(lo);
                    else Debug.Log($"Object {spawn.prefab.name} couldn't be spawned on {name}.");
                }
            }
        }

        private LevelObject SpawnFloorStandingObject(LevelObject prefab)
        {
            for (int j = 0; j < SpawnTries; j++)
            {
                float posX = SnapToGrid(Random.Range(GetLeftBoundaryX(prefab), GetRightBoundaryX(prefab)));
                Vector2 pos = new Vector2(posX, FloorSurfaceY);
                Vector2 checkPos = new Vector2(posX, GetBottomBoundary(prefab));
                if (Physics2D.OverlapBox(checkPos, prefab.Dim * CollCheckSizeScale, 0, _anyCollMask) == null)
                    return Instantiate(prefab, pos, Quaternion.identity, objectsParent);
            }
            return null;
        }
        
        private LevelObject SpawnCeilingHangingObject(LevelObject prefab)
        {
            for (int j = 0; j < SpawnTries; j++)
            {
                float posX = SnapToGrid(Random.Range(GetLeftBoundaryX(prefab), GetRightBoundaryX(prefab)));
                Vector2 pos = new Vector2(posX, CeilingSurfaceY);
                Vector2 checkPos = new Vector2(posX, GetTopBoundary(prefab));
                if (Physics2D.OverlapBox(checkPos, prefab.Dim * CollCheckSizeScale, 0, _anyCollMask) == null)
                    return Instantiate(prefab, pos, Quaternion.identity, objectsParent);
            }
            return null;
        }

        private LevelObject SpawnWallHangingObject(LevelObject prefab)
        {
            for (int j = 0; j < SpawnTries; j++)
            {
                float posX = SnapToGrid(Random.Range(GetLeftBoundaryX(prefab), GetRightBoundaryX(prefab)));
                float posY = SnapToGrid(Random.Range(GetBottomBoundary(prefab) + 1f, GetTopBoundary(prefab) - 1f));
                Vector2 pos = new Vector2(posX, posY);
                if (Physics2D.OverlapBox(pos, prefab.Dim * CollCheckSizeScale, 0, _anyCollMask) == null)
                    return Instantiate(prefab, pos, Quaternion.identity, objectsParent);
            }
            return null;
        }
    }
}