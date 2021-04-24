using UnityEditor;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(menuName = "Level Data Set", fileName = "Level Data Set")]
    public class LevelDataSet : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private LevelBlock floorPrefab;
        [SerializeField] private LevelBlock ceilingPrefab;
        [SerializeField] private LevelBlock backgroundPrefab;
        [SerializeField] private Staircase staircasePrefab;

        [Header("Values")]
        [SerializeField] private int length;
        [SerializeField] private float direction;
        [SerializeField] private float floorYOffset;
        [SerializeField] private float ceilingYOffset;


        public LevelBlock FloorPrefab => floorPrefab;
        public LevelBlock CeilingPrefab => ceilingPrefab;
        public LevelBlock BackgroundPrefab => backgroundPrefab;
        public Staircase StaircasePrefab => staircasePrefab;
        public int Length => length;
        public float Direction => direction;
        public float FloorYOffset => floorYOffset;
        public float CeilingYOffset => ceilingYOffset;
    }
}