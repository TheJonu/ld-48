using UnityEngine;

namespace Levels
{
    public enum PositionVariant
    {
        Standing, Hanging
    }

    public class LevelObject : MonoBehaviour
    {
        [SerializeField] private Vector2 dimensions;
        [SerializeField] private PositionVariant positionVariant;

        public Vector2 Dim => dimensions;
        public PositionVariant PositionVariant => positionVariant;
    }
}