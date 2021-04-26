using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityManager : MonoBehaviour
{
    [HideInInspector] public List<Levels.Level> allLevels;

    public Material[] materialSelection;
    public Material[] enemyMaterialSelection;

    public float insanityPercent;
    public float enemyInsanityPercent;

    public float insanityInc;
    public float enemyInsanityInc;

    private List<MatScript> mats = new List<MatScript>();
    // Start is called before the first frame update
    void Start()
    {
        if(materialSelection.Length == 0 || enemyMaterialSelection.Length == 0)
        {
            Debug.LogWarning("Not created insanity");
            Destroy(this);
        }

        foreach(Levels.Level lev in allLevels)
        {
            foreach(GameObject gm in lev.GetSprites())
            {
                float insanityPercent = gm.tag == "Enemy" ? this.enemyInsanityInc : this.insanityPercent;
                if (insanityPercent <= Random.Range(0f, 1f))
                {
                    continue;
                }

                Material select;

                if (gm.tag == "Enemy")
                    select = enemyMaterialSelection[Random.Range(0, enemyMaterialSelection.Length)];
                else
                    select = materialSelection[Random.Range(0, materialSelection.Length)];

                MatScript m = gm.AddComponent<MatScript>();

                m.newMaterial = select;

                mats.Add(m);
            }
            insanityPercent += insanityInc;
            enemyInsanityInc += enemyInsanityInc;
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
