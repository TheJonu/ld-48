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
    [SerializeField] private float insanityChance;
    public static LevelManager Instance { get; set; }
    public Transform Player => player;
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
        Staircase currentStaircase = firstStaircase;

        foreach (var levelDataSet in levelDataSets)
        {
            var level = Instantiate(levelPrefab, currentStaircase.ExitPos, Quaternion.identity, levelsParent);
            level.Data = levelDataSet;
            level.Entrance = currentStaircase;
            level.GenerateLevel();
            _levels.Add(level);
            currentStaircase = level.Exit;
        }

        StartInsanity();
    }

    private void StartInsanity()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (Level l in _levels)
        {
            List<GameObject> levellist = l.GetSprites();

            list.AddRange(levellist);
        }

        InsanityManager im = gameObject.AddComponent<InsanityManager>();

        im.allSprites = list;
        im.insanityPercent = insanityChance;
        im.materialSelection = insanityMaterials;
    }
}