using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollisionHandle : MonoBehaviour
{
    [HideInInspector] public static UnityEvent hitEvent;
    [HideInInspector] private static List<LocPair> enemies = new List<LocPair>();

    // Start is called before the first frame update
    void Start()
    {
        if(hitEvent == null)
        {
            hitEvent = new UnityEvent();
            hitEvent.AddListener(DoAThing);
        }

        enemies.Add(new LocPair(gameObject, gameObject.transform.position));
    }

    private struct LocPair
    {
        public GameObject gm;
        public Vector3 org;

        public LocPair(GameObject _gm, Vector3 _org)
        {
            gm = _gm;
            org = _org;
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
        CheckpointManager.GetInstance().resetPos.Invoke();
        /*
        foreach(LocPair lp in enemies)
        {
            lp.gm.transform.position = lp.org;
        }
        */
    }
}
