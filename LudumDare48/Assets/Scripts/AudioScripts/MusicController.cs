using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] clips;
    public float volume = 0.75f;
    private int curClip = 0;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.volume = volume;
        if (clips.Length == 0)
        {
            Destroy(this);
            return;
        }
    }

    private void OnDestroy()
    {
        Destroy(source);
    }

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
        {
            curClip = (curClip) + 1 % clips.Length;
            source.clip = clips[curClip];
            source.Play();
        }
    }
}
