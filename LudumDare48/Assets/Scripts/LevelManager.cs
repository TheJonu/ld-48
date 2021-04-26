using System;
using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private List<LevelDataSet> levelDataSets;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Level levelPrefab;
    [SerializeField] private Staircase firstStaircase;
    [SerializeField] private Transform levelsParent;

    [SerializeField] private Material[] insanityMaterials;
    [SerializeField] private Material[] insanityEnemyMaterials;

    [SerializeField] private float insanityChance;
    [SerializeField] private float perLevelIncrease;

    [SerializeField] private float enemyInsanityChance;
    [SerializeField] private float perLevelEnemyIncrease;

    public static LevelManager Instance { get; set; }
    public Transform Player => player;
    public float GridSize => gridSize;
    public Transform LevelsParent => levelsParent;
    public List<Level> AllLevels => _levels;
    public Level CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (value != _currentLevel)
            {
                _currentLevel = value;
                CurrentLevelChanged?.Invoke(_currentLevel);
                CheckpointManager.GetInstance().checkPointEvent.Invoke();
            }
        }
    }

    public Action<Level> CurrentLevelChanged { get; set; }


    private Level _currentLevel;
    private List<Level> _levels;
    

    private void Awake()
    {
        Instance = this;
        
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        _levels = new List<Level>();
        Staircase currentStaircase = firstStaircase;

        int counter = 1;
        foreach (var levelDataSet in levelDataSets)
        {
            var level = Instantiate(levelPrefab, currentStaircase.ExitPos, Quaternion.identity, levelsParent);
            level.Data = levelDataSet;
            level.Entrance = currentStaircase;
            level.name = $"Level {counter++}";
            level.GenerateLevel();
            _levels.Add(level);
            currentStaircase = level.Exit;
        }

        StartInsanity();
    }

    private void StartInsanity()
    {
        InsanityManager im = gameObject.AddComponent<InsanityManager>();

        im.allLevels = _levels;

        im.insanityPercent = insanityChance;
        im.enemyInsanityPercent = enemyInsanityChance;

        im.insanityInc = perLevelIncrease;
        im.enemyInsanityInc = perLevelEnemyIncrease;

        im.materialSelection = insanityMaterials;
        im.enemyMaterialSelection = insanityEnemyMaterials;
    }
}