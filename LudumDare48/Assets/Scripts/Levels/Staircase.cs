<<<<<<< Updated upstream
﻿
=======
﻿using UnityEngine;

namespace Levels
{
    public class Staircase : MonoBehaviour
    {
        [SerializeField] private Transform entrance;
        [SerializeField] private Transform exit;

        public Vector2 EntranceLocalPos => entrance.localPosition;
        
        public Vector2 EntrancePos => entrance.position;
        public Vector2 ExitPos => exit.position;
    }
}
>>>>>>> Stashed changes
