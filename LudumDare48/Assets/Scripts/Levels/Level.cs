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

        public LevelDataSet Data { get; set; }
        public Staircase Entrance { get; set; }
        public Staircase Exit { get; set; }

        private const int ObjectsBufferDist = 4; // how far from staircases can objects be spawned
        private const int SpawnTries = 100;

        private LayerMask _objectsMask;
        private List<LevelBlock> _blocks;
        private List<LevelObject> _objects;


        public void GenerateLevel()
        {
            _objectsMask = LayerMask.GetMask("Object");

            _blocks = new List<LevelBlock>();
            _objects = new List<LevelObject>();
            Vector2 endPos = Entrance.ExitPos + Data.Length * Data.FloorPrefab.Dim.x * Data.Direction * Vector2.right;

            GenerateFloor(Entrance.ExitPos, Data.Length, Data.FloorYOffset, Data.FloorPrefab);
            GenerateCeiling(Entrance.EntrancePos, Data.Length, Data.CeilingYOffset, Data.CeilingPrefab);
            GenerateExit(endPos, Data.StaircasePrefab);
            GenerateBackground(Entrance.ExitPos, Exit.CenterPosition, Data.BackgroundPrefab);
            GenerateStandingObjects(Data.LevelObjects.Where(o => o.prefab.PositionVariant == PositionVariant.Standing));
        }

        private void GenerateFloor(Vector2 startPos, int length, float offsetY, LevelBlock prefab)
        {
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(
                    prefab,
                    startPos + i * prefab.Dim.x * Data.Direction * Vector2.right + offsetY * Vector2.up,
                    Quaternion.identity,
                    floorParent);
                _blocks.Add(block);
            }
        }

        private void GenerateCeiling(Vector2 startPos, int length, float offsetY, LevelBlock prefab)
        {
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(
                    prefab,
                    startPos + i * prefab.Dim.x * Data.Direction * Vector2.right + offsetY * Vector2.up,
                    Quaternion.identity,
                    ceilingParent);
                _blocks.Add(block);
            }
        }

        private void GenerateExit(Vector2 pos, Staircase prefab)
        {
            Exit = Instantiate(prefab, pos, Quaternion.identity, LevelManager.Instance.LevelsParent);
            Exit.transform.Translate(-Exit.EntranceLocalPos);
            Exit.transform.Translate(Data.FloorPrefab.Dim.x * Data.Direction * Vector2.left);
        }

        private void GenerateBackground(Vector2 startPos, Vector2 endPos, LevelBlock prefab)
        {
            float y = startPos.y;
            int length = Mathf.RoundToInt(Mathf.Abs(endPos.x - startPos.x) / prefab.Dim.x) + 1;
            for (int i = 0; i < length; i++)
            {
                float x = startPos.x + i * Data.Direction * prefab.Dim.x;
                var block = Instantiate(prefab, new Vector2(x, y), Quaternion.identity, backgroundParent);
                _blocks.Add(block);
            }
        }

        private void GenerateStandingObjects(IEnumerable<SpawnAmount<LevelObject>> spawns)
        {
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
    }
}