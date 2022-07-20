using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    private double nextTime;

    private AudioSource audioSource;

    private float currentVol;

    private static Metronome i;

    private void Awake()
    {
        i = this;
        audioSource = GetComponent<AudioSource>();
        currentVol = audioSource.volume;
    }

    void Start()
    {
        nextTime = AudioSettings.dspTime + AudioController.beatPerSec;
    }

    void FixedUpdate()
    {
        if (AudioSettings.dspTime >= nextTime && audioSource.enabled)
        {
            audioSource.Play();
            nextTime += AudioController.beatPerSec;
        }
    }

    public static void UnMuteMetro()
    {
        i.UnMute();
    }

    public static void MuteMetro()
    {
        i.Mute();
    }

    private void Mute()
    {
        audioSource.volume = 0;
    }

    private void UnMute()
    {
        audioSource.volume = currentVol;
    }
}
