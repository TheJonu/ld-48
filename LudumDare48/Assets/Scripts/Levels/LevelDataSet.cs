using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Levels
{
    [System.Serializable]
    public class SpawnAmount<T> where T : MonoBehaviour
    {
        public T prefab;
        public int amount;
    }
    
    [CreateAssetMenu(menuName = "Level Data Set", fileName = "Level Data Set")]
    public class LevelDataSet : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private LevelBlock floorPrefab;
        [SerializeField] private LevelBlock ceilingPrefab;
        [SerializeField] private LevelBlock backgroundPrefab;
        [SerializeField] private Staircase staircasePrefab;
        [SerializeField] private List<SpawnAmount<LevelObject>> levelObjects;

        [Header("Values")]
        [SerializeField] private int length;
        [SerializeField] private int direction;
        [SerializeField] private int floorYOffset;
        [SerializeField] private int ceilingYOffset;
        [SerializeField] private float insanityLevel;


        public LevelBlock FloorPrefab => floorPrefab;
        public LevelBlock CeilingPrefab => ceilingPrefab;
        public LevelBlock BackgroundPrefab => backgroundPrefab;
        public Staircase StaircasePrefab => staircasePrefab;
        public List<SpawnAmount<LevelObject>> LevelObjects => levelObjects;
        public int Length => length;
        public int Direction => direction;
        public int FloorYOffset => floorYOffset;
        public int CeilingYOffset => ceilingYOffset;
        public float InsanityLevel => insanityLevel;
    }
}