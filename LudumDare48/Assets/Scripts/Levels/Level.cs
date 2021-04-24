using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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
        [SerializeField] private BoxCollider2D floorBoxCollider;
        
        public LevelDataSet Data { get; set; }
        public Staircase Entrance { get; set; }
        public Staircase Exit { get; set; }

        private const int ObjectsBufferDist = 3; // how far from staircases can objects be spawned
        private const int SpawnTries = 100;

        private LayerMask _objectsMask;
        private LayerMask _terrainMask;
        private List<LevelBlock> _blocks;
        private List<LevelObject> _objects;


        public void GenerateLevel()
        {
            _objectsMask = LayerMask.GetMask("Object");
            _terrainMask = LayerMask.GetMask("Terrain");

            _blocks = new List<LevelBlock>();
            _objects = new List<LevelObject>();
            
            GenerateExit();
            
            GenerateFloor();
            GenerateCeiling();
            GenerateBackground();
            
            GenerateStandingObjects();
            GenerateHangingObjects();
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
            floorBoxCollider.transform.position = startPos + centerDist;
            floorBoxCollider.size = new Vector2(length * Data.FloorPrefab.Dim.x, Data.FloorPrefab.Dim.y);
        }

        private void GenerateCeiling()
        {
            Vector2 startPos = new Vector2(Entrance.ExitPos.x, Entrance.EntrancePos.y + Data.CeilingYOffset);
            Vector2 blockDist = Data.CeilingPrefab.Dim.x * Data.Direction * Vector2.right;
            int length = Data.Length + Mathf.RoundToInt(Exit.Dim.x);
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(Data.CeilingPrefab, startPos + i * blockDist, Quaternion.identity, ceilingParent);
                _blocks.Add(block);
            }
        }

        private void GenerateExit()
        {
            Vector2 exitPos = Entrance.ExitPos + Data.Length * Data.FloorPrefab.Dim.x * Data.Direction * Vector2.right;
            Exit = Instantiate(Data.StaircasePrefab, exitPos, Quaternion.identity, LevelManager.Instance.LevelsParent);
            Exit.transform.Translate(-Exit.EntranceLocalPos);
            Exit.transform.Translate(Data.FloorPrefab.Dim.x * Data.Direction * Vector2.left);
        }

        private void GenerateBackground()
        {
            float y = Entrance.ExitPos.y;
            float startPosX = Entrance.ExitPos.x - Data.Direction * Entrance.Dim.x;
            float endPosX = Exit.EntrancePos.x + Data.Direction * Exit.Dim.x;
            int length = Mathf.RoundToInt(Mathf.Abs(endPosX - startPosX) / Data.BackgroundPrefab.Dim.x) + 1;
            for (int i = 0; i < length; i++)
            {
                float x = startPosX + i * Data.Direction * Data.BackgroundPrefab.Dim.x;
                var block = Instantiate(Data.BackgroundPrefab, new Vector2(x, y), Quaternion.identity, backgroundParent);
                _blocks.Add(block);
            }
        }

        private void GenerateStandingObjects()
        {
            var spawns = Data.LevelObjects.Where(o => o.prefab.PositionVariant == PositionVariant.Standing);
            float floorSurfacePosY = Entrance.ExitPos.y + 0.5f;
            int leftBoundary = Mathf.RoundToInt(Entrance.ExitPos.x) + Data.Direction * ObjectsBufferDist;
            int rightBoundary = Mathf.RoundToInt(Exit.EntrancePos.x) - Data.Direction * ObjectsBufferDist;
            foreach (var spawn in spawns)
            {
                for (int i = 0; i < spawn.amount; i++)
                {
                    Vector2 size = new Vector2(spawn.prefab.Dim.x, spawn.prefab.Dim.y);
                    Vector2 pos = Vector2.zero;
                    bool ok = false;
                    for (int j = 0; j < SpawnTries; j++)
                    {
                        pos = new Vector2(Random.Range(leftBoundary, rightBoundary), floorSurfacePosY + size.y / 2);
                        if (Physics2D.OverlapBox(pos, size, 0, _objectsMask) == null)
                        {
                            ok = true;
                            break;
                        }
                        if (j == 49)
                            Debug.Log($"Object {spawn.prefab.name} couldn't be spawned.");
                    }
                    if (ok)
                    {
                        var instance = Instantiate(spawn.prefab, pos, Quaternion.identity, objectsParent);
                        _objects.Add(instance);
                    }
                }
            }
        }
        
        private void GenerateHangingObjects()
                {
                    var spawns = Data.LevelObjects.Where(o => o.prefab.PositionVariant == PositionVariant.Hanging);
                    int leftBoundary = Mathf.RoundToInt(Entrance.ExitPos.x) + Data.Direction * ObjectsBufferDist;
                    int rightBoundary = Mathf.RoundToInt(Exit.EntrancePos.x) - Data.Direction * ObjectsBufferDist;
                    int bottomBoundary = Mathf.RoundToInt(Entrance.ExitPos.y) + 2;
                    int topBoundary = Mathf.RoundToInt(Entrance.EntrancePos.y + Data.CeilingYOffset) - 1;
                    foreach (var spawn in spawns)
                    {
                        for (int i = 0; i < spawn.amount; i++)
                        {
                            Vector2 size = new Vector2(spawn.prefab.Dim.x, spawn.prefab.Dim.y);
                            Vector2 pos = Vector2.zero;
                            bool ok = false;
                            for (int j = 0; j < SpawnTries; j++)
                            {
                                pos = new Vector2(Random.Range(leftBoundary, rightBoundary), Random.Range(bottomBoundary, topBoundary));
                                if (Physics2D.OverlapBox(pos, size, 0, _objectsMask + _terrainMask) == null)
                                {
                                    ok = true;
                                    break;
                                }
                                if (j == 49)
                                    Debug.Log($"Object {spawn.prefab.name} couldn't be spawned.");
                            }
                            if (ok)
                            {
                                var instance = Instantiate(spawn.prefab, pos, Quaternion.identity, objectsParent);
                                _objects.Add(instance);
                            }
                        }
                    }
                }
    }
}