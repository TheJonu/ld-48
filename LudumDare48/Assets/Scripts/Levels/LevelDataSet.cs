using UnityEditor;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(menuName = "Level Data Set", fileName = "Level Data Set")]
    public class LevelDataSet : ScriptableObject
    {
        [SerializeField] private LevelBlock floorBlockPrefab;
        [SerializeField] private Staircase nextStaircasePrefab;
        [SerializeField] private int length;
        [SerializeField] private float direction;

        public LevelBlock FloorBlockPrefab => floorBlockPrefab;
        public Staircase NextStaircasePrefab => nextStaircasePrefab;
        public int Length => length;
        public float Direction => direction;
    }
}