using UnityEditor;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(menuName = "Level Data Set", fileName = "Level Data Set")]
    public class LevelDataSet : ScriptableObject
    {
        [SerializeField] private LevelBlock _floorBlockPrefab;
        [SerializeField] private Staircase _nextStaircasePrefab;
        [SerializeField] private int _length;
        [SerializeField] private float _direction;

        public LevelBlock FloorBlockPrefab => _floorBlockPrefab;
        public Staircase NextStaircasePrefab => _nextStaircasePrefab;
        public int Length => _length;
        public float Direction => _direction;
    }
}