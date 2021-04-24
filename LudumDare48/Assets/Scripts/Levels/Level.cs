<<<<<<< Updated upstream
﻿namespace Levels
{
    public class Level
    {
        
=======
﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels
{
    
    
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform floorParent;

        public LevelDataSet LevelDataSet { get; set; }
        public Staircase ExitStaircase => _exit;
        
        private float GridSize => LevelManager.Instance.GridSize;
        private int Length => LevelDataSet.Length;
        private float Direction => LevelDataSet.Direction;
        
        private List<LevelBlock> _floor;
        private Staircase _exit;
        

        public void Generate()
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + Length * GridSize * Direction * Vector2.right;
            GenerateFloor(startPos, Length,  -1f, LevelDataSet.FloorBlockPrefab);
            GenerateExit(endPos);
        }

        private void GenerateFloor(Vector2 startPos, int length, float offsetY, LevelBlock floorBlockPrefab)
        {
            _floor = new List<LevelBlock>();
            for (int i = 0; i < length; i++)
            {
                var block = Instantiate(
                    floorBlockPrefab, 
                    startPos + GridSize * Direction * i * Vector2.right + offsetY * Vector2.up,
                    Quaternion.identity, floorParent);
                _floor.Add(block);
            }
        }

        private void GenerateExit(Vector2 pos)
        {
            _exit = Instantiate(LevelDataSet.NextStaircasePrefab, pos, Quaternion.identity, LevelManager.Instance.LevelsParent);
            _exit.transform.Translate(-_exit.EntranceLocalPos);
            _exit.transform.Translate(GridSize * Direction * Vector2.left);
        }
>>>>>>> Stashed changes
    }
}