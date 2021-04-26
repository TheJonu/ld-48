using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject playerLocation;
    [SerializeField] private AudioClip[] backgroundTracks;
    [SerializeField] private float volume = 0.77f;
    [SerializeField] private List<AudioClip> pinkNoiseClips;

    public static MusicManager Instance { get; private set; }
    public List<AudioClip> PinkNoiseClips => pinkNoiseClips;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HookMusicPlayer();
    }

    private void HookMusicPlayer()
    {
        //MusicController mc = playerLocation.AddComponent<MusicController>();
        MusicController mc = Camera.main.gameObject.AddComponent<MusicController>(); // camera is the audio listener

        mc.clips = backgroundTracks;
        mc.volume = volume;
    }
}
