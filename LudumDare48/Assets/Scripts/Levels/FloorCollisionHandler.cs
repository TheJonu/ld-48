using System;
using UnityEngine;

namespace Levels
{
     public class FloorCollisionHandler : MonoBehaviour
     {
          [SerializeField] private BoxCollider2D collider;
     
          public BoxCollider2D Collider => collider;
          public Action EnteredCollision { get; set; }

          private const string PlayerTag = "Player";
     

          private void OnCollisionEnter2D(Collision2D collision)
          {
               if (collision.gameObject.CompareTag(PlayerTag))
               {
                    EnteredCollision?.Invoke();
               }
          }
     }
}
