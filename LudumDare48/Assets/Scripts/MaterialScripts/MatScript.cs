using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatScript : MonoBehaviour
{

    public Material newMaterial;
    private Material oldMaterial;

    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        oldMaterial = rend.material;
        rend.material = newMaterial;
    }

    private void OnDestroy()
    {
        rend.material = oldMaterial;
    }
}
