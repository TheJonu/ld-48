using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] clips;
    public float volume = 0.75f;
    private int curClip = 0;
    private AudioSource source;
    private AudioSource trans;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>() as AudioSource;
        trans = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.volume = 0.0f;
        source.clip = clips[curClip];
        source.Play();
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

    public void Next()
    {
        if (curClip == clips.Length - 1)
        {
            return;
        }
        trans.clip = clips[curClip];
        trans.time = source.time;
        trans.volume = volume;
        trans.Play();

        curClip++;
        source.clip = clips[curClip];
        if (curClip == 1) {
            source.time *= 3.0f;
        }
        source.volume = 0.0f;
        source.Play();

        Debug.Log(curClip);
    }

    void FixedUpdate()
    {
        if (trans.isPlaying)
        {
            trans.volume -= Time.fixedDeltaTime * volume * 0.2f;
        }
        if(source.volume < volume)
        {
            source.volume += Time.fixedDeltaTime * volume * 0.2f;
        }
        else
        {
            source.volume = volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
        {
            source.clip = clips[curClip];
            source.Play();
        }
    }
}
