using System;
using System.Collections;
using System.Collections.Generic;
using AudioHelm;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public static float beatPerSec;

    private AudioHelmClock helmClock;

    private HelmController helmController;

    void Awake()
    {
        helmClock = GetComponent<AudioHelmClock>();
        helmController = GetComponent<HelmController>();

        MIDIController.NoteOn += PlaySound;
        MIDIController.NoteOff += NoteOff;

        beatPerSec = 60 / helmClock.bpm;
    }

    void Update()
    {
        beatPerSec = 60 / helmClock.bpm;
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    private void PlaySound(int note, float velocity)
    {
        helmController.NoteOn (note, velocity);
    }

    private void NoteOff(int note)
    {
        helmController.NoteOff (note);
    }
}
