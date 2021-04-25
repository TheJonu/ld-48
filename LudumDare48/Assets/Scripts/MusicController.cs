using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.clip = clips[0];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
