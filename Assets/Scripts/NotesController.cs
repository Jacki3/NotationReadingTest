using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private TimeLine timeLine;

    [SerializeField]
    private List<Note> allNotes = new List<Note>();

    private float grandStaffDefaultDistY;

    private float grandStaffDefaultDistX;

    private float defaultXPos;

    private bool newStaffBool = false;

    private bool newBar = false;

    public static float distanceY;

    public static int totalNotes;

    public static int totalStaffs_Static;

    private int noteIndex = 0;

    [Serializable]
    private class Note
    {
        public int note;

        public int noteLength;

        public bool isEighth;
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

        //look at sasr again for scoring then implement something similar
        //ensure users can play and this saves and same as flash card; track if they quit before and only save score if they finish a whole piece n save this value
        //hook up total complete ui to stats
        //write to file
        //upload to server once you know how this is done
        //flats/sharps (use the game logic)
        //calculate all ypositions in logic using .11 as diff between notes (this may change if you change size of staff etc)
        //if time, add a more faded colour on the non notes part of staff
        //if there is time then add eighth notes properly
    }

    private void PlayNote(int note, float vel)
    {
        if (StateController.currentState == StateController.States.play)
        {
            if (note == allNotes[noteIndex].note)
                print("Correct!");
            else
                print("Incorrect Note!");

            //create a whole note prefab at the y played and at the current x position of the timeline - this x position can determine score
        }
    }

    private void NoteOff(int note)
    {
    }

    public void SpawnNotes()
    {
        int notesInBar = 0;
        int notesInStaff = 0;
        float staffYDist = 0;
        int totalStaffs = 1;
        Transform staffParent = firstParent;
        foreach (Note note in allNotes)
        {
            if (note.isEighth) note.noteLength = 1;
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
            notePrefab.SetNoteText(note.noteLength, note.isEighth);
            notePrefab.SetLedgerLine(note.note);
            float posY = notePrefab.ySpawns[note.note % 12] - staffYDist;

            if (note.note < 60)
            {
                //using bass note
                foreach (Transform child in staffParent)
                {
                    if (child.tag == "Break") child.gameObject.SetActive(false);
                }
                posY -= 1.783f;
            }

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

            notePrefab.transform.localPosition =
                new Vector3(startingXPos, posY, 0);

            if (notesInBar == 4)
            {
                // newBar = true;
                float barXPos =
                    notePrefab.transform.localPosition.x +
                    note.noteLength -
                    barDistance;
                GameObject newBarLine =
                    Instantiate(barLine, staffParent, false);
                float barYPos =
                    newBarLine.transform.localPosition.y - staffYDist;
                newBarLine.transform.localPosition =
                    new Vector3(barXPos, barYPos, 0);
                notesInBar = 0;
            }

            if (notesInStaff % 16 == 0)
            {
                float staffPosY = grandStaffDefaultDistY - grandStaffDistanceY;
                grandStaffDefaultDistY = staffPosY;

                GameObject newStaff = Instantiate(grandStaff, transform);
                staffParent = newStaff.transform;
                foreach (Transform child in staffParent)
                {
                    if (child.tag == "Break") child.gameObject.SetActive(true);
                }
                newStaff.transform.localPosition =
                    new Vector3(grandStaffDefaultDistX, staffPosY, 1);

                totalStaffs++;
                totalStaffs_Static = totalStaffs;
                startingXPos = defaultXPos;
                newStaffBool = true;
            }
        }
    }

    public void StartPlay()
    {
        StartCoroutine(PlayAlong());
    }

    private IEnumerator PlayAlong()
    {
        while (true)
        {
            yield return new WaitForSeconds(AudioController.beatPerSec *
                    allNotes[noteIndex].noteLength);
            if (noteIndex < allNotes.Count - 1)
                noteIndex++;
            else
                break;
        }
    }
}
