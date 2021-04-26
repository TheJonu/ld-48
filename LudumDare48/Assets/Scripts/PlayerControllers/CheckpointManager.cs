using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 org;
    [HideInInspector] public UnityEvent checkPointEvent;
    [HideInInspector] public UnityEvent resetPos;
    
    public static CheckpointManager Instance { get; private set; }
    public Action ReturnedToCheckpoint;
    
    // Start is called before the first frame update

    public static CheckpointManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        org = gameObject.transform.position;
        checkPointEvent.AddListener(SetNew);
        resetPos.AddListener(ResetPos);
        resetPos.AddListener(() => ReturnedToCheckpoint?.Invoke());
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
