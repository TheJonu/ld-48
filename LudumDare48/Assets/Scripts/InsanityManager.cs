using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanityManager : MonoBehaviour
{
    [HideInInspector] public List<Levels.Level> allLevels;

    public Material[] materialSelection;
    public Material[] enemyMaterialSelection;

    public GameObject playerLocation;

    public float insanityPercent;
    public float enemyInsanityPercent;

    public float insanityInc;
    public float enemyInsanityInc;

    private List<MatScript> mats = new List<MatScript>();

    private float spawnTimer;
    private float despawnTimer;
    private Levels.Level prevLev = null;
    private bool firstLevel = true;

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        despawnTimer += Time.deltaTime;

        if(despawnTimer > 0.15f)
        {
            despawnTimer = 0.0f;
            foreach (MatScript mat in mats)
            {
                if (Random.Range(0f, 1f) > 0.3f)
                {
                    mats.Remove(mat);
                    Destroy(mat);
                    break;
                }
            }
        }

        if(spawnTimer > 0.5f)
        {
            spawnTimer = 0.0f;
            Levels.Level lev = LevelManager.Instance.CurrentLevel;
            if(!lev)
            {
                return;
            }
            if(lev != prevLev)
            {
                prevLev = lev;
                if(firstLevel)
                {
                    firstLevel = false;
                } 
                else
                {
                    Debug.Log("Insanity increased");
                    insanityPercent += insanityInc / 30.0f + insanityPercent / 30.0f;
                    enemyInsanityInc += enemyInsanityInc / 30.0f + enemyInsanityInc / 30.0f;
                    playerLocation.GetComponent<MusicController>().Next();
                }
            }
            foreach (GameObject gm in lev.GetSprites())
            {
                float insanityPercent = gm.tag == "Enemy" ? this.enemyInsanityInc : this.insanityPercent;
                if (Random.Range(0f, 1f) >= insanityPercent)
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(materialSelection.Length == 0 || enemyMaterialSelection.Length == 0)
        {
            Debug.LogWarning("Not created insanity");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        foreach(MatScript mat in mats)
        {
            Destroy(mat);
        }
    }
}
