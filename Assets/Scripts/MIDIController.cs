using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MIDIController : MonoBehaviour
{
    public static int startingMIDINumber = 36;

    public delegate void NoteOnEventHandler(int note, float velocity);

    public static event NoteOnEventHandler NoteOn;

    public delegate void NoteOffEventHandler(int note);

    public static event NoteOffEventHandler NoteOff;

    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                NoteOn(note.noteNumber, velocity);
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                NoteOff(note.noteNumber);
            };
        };
    }
}
