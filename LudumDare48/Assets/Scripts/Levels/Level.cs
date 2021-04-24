using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform floorParent;

        public LevelDataSet LevelDataSet { get; set; }
        public Staircase ExitStaircaseStaircase => _exitStaircase;
        
        private float GridSize => LevelManager.Instance.GridSize;
        private int Length => LevelDataSet.Length;
        private float Direction => LevelDataSet.Direction;
        
        private List<LevelBlock> _floorBlocks;
        private Staircase _exitStaircase;
        

        public void GenerateLevel()
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + Length * GridSize * Direction * Vector2.right;
            GenerateFloor(startPos, Length, LevelDataSet.FloorBlockPrefab);
            GenerateExit(endPos);
        }

        private void GenerateFloor(Vector2 startPos, int length, LevelBlock floorBlockPrefab)
        {
            _floorBlocks = new List<LevelBlock>();
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(
                    floorBlockPrefab, 
                    startPos + GridSize * Direction * i * Vector2.right,
                    Quaternion.identity, floorParent);
                _floorBlocks.Add(block);
            }
        }

        private void GenerateExit(Vector2 pos)
        {
            _exitStaircase = Instantiate(LevelDataSet.NextStaircasePrefab, pos, Quaternion.identity, LevelManager.Instance.LevelsParent);
            _exitStaircase.transform.Translate(-_exitStaircase.EntranceLocalPos);
            _exitStaircase.transform.Translate(GridSize * Direction * Vector2.left);
        }
    }
}