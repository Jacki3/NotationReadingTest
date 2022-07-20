using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesController : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField]
    private float startingXPos;

    [SerializeField]
    private float defaultXDistance;

    [SerializeField]
    private float barDistance = 2;

    [SerializeField]
    private float grandStaffDistanceY;

    [Header("Components")]
    [SerializeField]
    private GameObject barLine;

    [SerializeField]
    private GameObject grandStaff;

    [SerializeField]
    private Transform firstParent;

    [SerializeField]
    private NotePrefab _notePrefab;

    [SerializeField]
    private List<Note> allNotes = new List<Note>();

    [SerializeField]
    private NotePrefab[] notePrefabs;

    private float grandStaffDefaultDistY;

    private float grandStaffDefaultDistX;

    private float defaultXPos;

    private bool newStaffBool = false;

    private bool newBar = false;

    public static float distanceY;

    public static int totalNotes;

    [Serializable]
    private class Note
    {
        public int note;

        public int noteLength;
    }

    void Awake()
    {
        MIDIController.NoteOn += PlayNote;
        MIDIController.NoteOff += NoteOff;
    }

    private void Start()
    {
        defaultXPos = startingXPos;
        grandStaffDefaultDistY = firstParent.transform.localPosition.y;
        grandStaffDefaultDistX = firstParent.transform.localPosition.x;
        distanceY = grandStaffDistanceY;
        SpawnNotes();

        //change the note prefab to text and rework the logic below - finalise this!
        //add all positions/ledgers etc. and ensure that the note is correct (i.e., 24 NOT 0 but still uses right pos)
        //ensure scrolling works and stopping when finished works
        //hook up total complete ui to stats - ensure users can play and this saves and same as flash card; track if they quit before and only save score if they finish a whole piece n save this value (LATER)
        //add break symbols
        //look at sasr again for scoring then implement something similar
        //if time, add a more faded colour on the non notes part of staff
        //write to file
        //upload to server once you know how this is done
    }

    private void PlayNote(int note, float vel)
    {
        print (note);
    }

    private void NoteOff(int note)
    {
    }

    private void SpawnNotes()
    {
        int notesInBar = 0;
        int notesInStaff = 0;
        float staffYDist = 0;
        int totalStaffs = 1;
        Transform staffParent = firstParent;
        foreach (Note note in allNotes)
        {
            totalNotes += note.noteLength;

            Note previousNote = null;
            float distMultiplier = 1;

            notesInBar += note.noteLength;
            notesInStaff += note.noteLength;

            if (allNotes.IndexOf(note) != 0)
            {
                previousNote = allNotes[allNotes.IndexOf(note) - 1];
                distMultiplier = previousNote.noteLength;
            }

            NotePrefab notePrefab =
                Instantiate(_notePrefab, staffParent, false);
            notePrefab.SetNoteText(note.noteLength);
            notePrefab.SetLedgerLine(note.note);
            float posY = notePrefab.ySpawns[note.note] - staffYDist;

            if (newBar)
            {
                newBar = false;
                distMultiplier = barDistance;
            }

            if (allNotes.IndexOf(note) != 0)
            {
                if (!newStaffBool)
                    startingXPos += defaultXDistance * distMultiplier;
                else
                    newStaffBool = false;
            }

            notePrefab.transform.position =
                new Vector3(startingXPos, posY, .5f);

            if (notesInBar == 4)
            {
                // newBar = true;
                float barXPos =
                    notePrefab.transform.position.x +
                    note.noteLength -
                    barDistance;
                GameObject newBarLine =
                    Instantiate(barLine, staffParent, false);
                float barYPos =
                    newBarLine.transform.localPosition.y - staffYDist;
                newBarLine.transform.position =
                    new Vector3(barXPos, barYPos, .5f);
                notesInBar = 0;
                // startingXPos = barXPos;
            }

            if (notesInStaff % 16 == 0)
            {
                float staffPosY = grandStaffDefaultDistY - grandStaffDistanceY;
                grandStaffDefaultDistY = staffPosY;

                GameObject newStaff = Instantiate(grandStaff, transform);
                staffParent = newStaff.transform;
                newStaff.transform.localPosition =
                    new Vector3(grandStaffDefaultDistX, staffPosY, 1);

                staffYDist = grandStaffDistanceY * totalStaffs;
                totalStaffs++;
                startingXPos = defaultXPos;
                newStaffBool = true;
            }
        }
    }
}
