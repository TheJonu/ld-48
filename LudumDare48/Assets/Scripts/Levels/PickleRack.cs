using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Levels
{
    public class PickleRack : MonoBehaviour
    {
        [SerializeField] private CollisionHandler collisionHandler;
        [SerializeField] private List<GameObject> toDisable;
        [SerializeField] private List<GameObject> toEnable;
        
        
        public static PickleRack Instance { get; private set; }
        public Action PickleAcquired;
        public Action GameEnded;

        private GameObject Player => GameObject.FindWithTag("Player");
        private CanvasGroup Fade => GameObject.FindWithTag("Fade").GetComponent<CanvasGroup>();
        
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
                StartCoroutine(EndRoutine());
            }
        }

        private IEnumerator EndRoutine()
        {
            yield return new WaitForSeconds(3f);
            
            Fade.DOFade(1f, 2f);
            
            yield return new WaitForSeconds(2.5f);
            
            toDisable.ForEach(go => go.SetActive(false));
            Player.SetActive(false);
            toEnable.ForEach(go => go.SetActive(true));

            yield return new WaitForSeconds(0.5f);

            Fade.DOFade(0f, 3f);

            yield return new WaitForSeconds(6f);
            
            GameEnded?.Invoke();
        }
    }
}