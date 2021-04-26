using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 org;
    [HideInInspector] public UnityEvent checkPointEvent;
    [HideInInspector] public UnityEvent resetPos;
    private static CheckpointManager instance;
    // Start is called before the first frame update

    public static CheckpointManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
        org = gameObject.transform.position;
        checkPointEvent.AddListener(SetNew);
        resetPos.AddListener(ResetPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNew()
    {
        org = gameObject.transform.position;
    }

    void ResetPos()
    {
        gameObject.transform.position = org;
    }
}
