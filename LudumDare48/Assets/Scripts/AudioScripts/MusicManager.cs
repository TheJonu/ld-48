using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject playerLocation;
    [SerializeField] private AudioClip[] backgroundTracks;
    [SerializeField] private List<AudioClip> pinkNoiseClips;
    [SerializeField] public Slider volumeSlider;
    private MusicController mc;

    public static MusicManager Instance { get; private set; }
    public List<AudioClip> PinkNoiseClips => pinkNoiseClips;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HookMusicPlayer();
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.value); });
    }

    private void ChangeVolume(float val)
    {
        mc.ChangeVolume(val);
    }

    private void HookMusicPlayer()
    {
        //MusicController mc = playerLocation.AddComponent<MusicController>();
        mc = Camera.main.gameObject.AddComponent<MusicController>(); // camera is the audio listener

        mc.clips = backgroundTracks;
        mc.volume = volumeSlider.value;
    }
}
