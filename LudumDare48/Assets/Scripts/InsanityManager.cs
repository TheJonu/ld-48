using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> allSprites;

    public Material[] materialSelection;

    public float insanityPercent;

    private List<MatScript> mats = new List<MatScript>();
    // Start is called before the first frame update
    void Start()
    {
        if(materialSelection.Length == 0)
        {
            Debug.LogWarning("Not created insanity");
            Destroy(this);
        }

        foreach(GameObject gm in allSprites)
        {
            if(insanityPercent <= Random.Range(0f, 1f))
            {
                continue;
            }
            Material select = materialSelection[Random.Range(0, materialSelection.Length)];

            MatScript m = gm.AddComponent<MatScript>();

            m.newMaterial = select;

            mats.Add(m);
        }
    }

    private void OnDestroy()
    {
        foreach(MatScript mat in mats)
        {
            Destroy(mat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
