using UnityEditor;
using UnityEngine;

namespace Levels
{
<<<<<<< Updated upstream
    [CreateAssetMenu(menuName = "Level Data", fileName = "Level Data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private LevelBlock _floorBlockPrefab;
        public LevelBlock FloorBlockPrefab => _floorBlockPrefab;
        
=======
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
>>>>>>> Stashed changes
    }
}