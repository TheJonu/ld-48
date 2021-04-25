using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject playerLocation;
    [SerializeField] private AudioClip[] backgroundTracks;
    [SerializeField] private float volume = 0.77f;
    // Start is called before the first frame update
    void Start()
    {
        HookMusicPlayer();
    }

    private void HookMusicPlayer()
    {
        MusicController mc = playerLocation.AddComponent<MusicController>();

        mc.clips = backgroundTracks;
        mc.volume = volume;
    }
}
