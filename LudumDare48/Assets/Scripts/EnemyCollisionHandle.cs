using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollisionHandle : MonoBehaviour
{
    [HideInInspector] public static UnityEvent hitEvent;
    // Start is called before the first frame update
    void Start()
    {
        if(hitEvent == null)
        {
            hitEvent = new UnityEvent();
            hitEvent.AddListener(DoAThing);
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.name == "Player")
        {
            hitEvent.Invoke();
        }
    }

    void DoAThing()
    {
        // reset a level, call a chechpoint whatever
    }
}
