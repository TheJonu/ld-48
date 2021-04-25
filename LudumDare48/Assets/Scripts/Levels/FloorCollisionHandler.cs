using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
