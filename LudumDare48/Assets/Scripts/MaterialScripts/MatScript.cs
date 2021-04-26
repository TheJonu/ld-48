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
        
        //AudioSource.PlayClipAtPoint(MusicManager.Instance.PinkNoiseClips.GetRandom(), transform.position);
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = MusicManager.Instance.PinkNoiseClips.GetRandom();
        audioSource.volume = Mathf.Lerp(1f, 0f, Vector2.Distance(transform.position, Camera.main.transform.position) / 12f);
        //audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        //audioSource.maxDistance = 1f;
        audioSource.Play();
        Destroy(audioSource, 1f);
    }

    private void OnDestroy()
    {
        rend.material = oldMaterial;
    }
}
