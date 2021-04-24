using UnityEngine;

namespace Levels
{
    public class LevelBlock : MonoBehaviour
    {
        [SerializeField] private Vector2 dimensions;

        public Vector2 Dim => dimensions;
    }
}