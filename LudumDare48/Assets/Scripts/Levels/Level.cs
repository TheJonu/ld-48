using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform floorParent;
        [SerializeField] private Transform ceilingParent;
        [SerializeField] private Transform backgroundParent;

        
        public LevelDataSet Data { get; set; }
        public Staircase Entrance { get; set; }
        public Staircase Exit { get; set; }
        

        private List<LevelBlock> _blocks;


        public void GenerateLevel()
        {
            _blocks = new List<LevelBlock>();
            Vector2 endPos = Entrance.ExitPos + Data.Length * Data.FloorPrefab.Dim.x * Data.Direction * Vector2.right;
            
            GenerateFloor(Entrance.ExitPos, Data.Length, Data.FloorYOffset, Data.FloorPrefab);
            GenerateCeiling(Entrance.EntrancePos, Data.Length, Data.CeilingYOffset, Data.CeilingPrefab);
            GenerateExit(endPos, Data.StaircasePrefab);
            GenerateBackground(Entrance.CenterPosition, Exit.CenterPosition, Data.BackgroundPrefab);
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
            _blocks.Add(Exit);
        }

        private void GenerateBackground(Vector2 startPos, Vector2 endPos, LevelBlock prefab)
        {
            float y = startPos.y;
            int length = Mathf.RoundToInt(Mathf.Abs(endPos.x - startPos.x) / prefab.Dim.x) + 1;
            for (int i=0; i < length; i++)
            {
                float x = startPos.x + i * Data.Direction * prefab.Dim.x;
                var block = Instantiate(prefab, new Vector2(x, y), Quaternion.identity, backgroundParent);
                _blocks.Add(block);
            }
        }
    }
}