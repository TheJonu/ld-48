<<<<<<< Updated upstream
﻿namespace DefaultNamespace
{
    public class LevelManager
    {
        
=======
﻿using System;
using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelDataSet> levelDataSets;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Level levelPrefab;
    [SerializeField] private Staircase firstStaircase;
    [SerializeField] private Transform levelsParent;

    public static LevelManager Instance { get; set; }
    public float GridSize => gridSize;
    public Transform LevelsParent => levelsParent;


    private List<Level> _levels;
        

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _levels = new List<Level>();
        Vector2 newPos = firstStaircase.ExitPos;

        foreach (var levelDataSet in levelDataSets)
        {
            var level = Instantiate(levelPrefab, newPos, Quaternion.identity, levelsParent);
            level.LevelDataSet = levelDataSet;
            level.Generate();
            newPos = level.ExitStaircase.ExitPos;
        }
>>>>>>> Stashed changes
    }
}