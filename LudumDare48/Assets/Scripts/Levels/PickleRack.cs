using System;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Levels
{
    public class PickleRack : MonoBehaviour
    {
        [SerializeField] private CollisionHandler collisionHandler;
        
        public static PickleRack Instance { get; private set; }
        public Action PickleAcquired;

        private bool _pickleAcquired;
        
        
        private void Awake()
        {
            Instance = this;

            collisionHandler.EnteredTrigger += AcquirePickle;
        }

        private void AcquirePickle()
        {
            if (_pickleAcquired == false)
            {
                _pickleAcquired = true;
                PickleAcquired?.Invoke();
            }
        }
    }
}