using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.SceneManagement;

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
    public bool preTest = true;

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
    public List<Note> allNotes = new List<Note>();

    [SerializeField]
    public List<AllNotes> noteLevels = new List<AllNotes>();

    private float grandStaffDefaultDistY;

    private float grandStaffDefaultDistX;

    private float defaultXPos;

    private bool newStaffBool = false;

    private bool newBar = false;

    public static float distanceY;

    public static int totalNotes;

    public static int totalStaffs_Static;

    public int noteIndex = 0;

    public int incorrectPerNote;

    public List<Transform> spawedNotes = new List<Transform>();

    private List<NotePrefab> playedNotes = new List<NotePrefab>();

    [Serializable]
    public class Note
    {
        public int note;

        public int noteLength;

        public bool guessedCorrectly;

        public int incorretGuessesMade;

        public string speed;

        public bool isEighth;
    }

    [Serializable]
    public class AllNotes
    {
        public string levelName;

        public List<Note> allNotes = new List<Note>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += ResetStaticVariables;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetStaticVariables;
        MIDIController.NoteOn -= PlayNote;
        MIDIController.NoteOff -= NoteOff;
    }

    private void ResetStaticVariables(Scene scene, LoadSceneMode mode)
    {
        distanceY = 0;
        totalNotes = 0;
        totalStaffs_Static = 0;
    }

    void Awake()
    {
        MIDIController.NoteOn += PlayNote;
        MIDIController.NoteOff += NoteOff;

        allNotes = noteLevels[GameController.level].allNotes.ToList();
    }

    private void Start()
    {
        defaultXPos = startingXPos;
        grandStaffDefaultDistY = firstParent.transform.localPosition.y;
        grandStaffDefaultDistX = firstParent.transform.localPosition.x;
        distanceY = grandStaffDistanceY;

        int currentLevel = (PlayerPrefs.GetInt("LevelsComplete"));
        string currentTitle = noteLevels[currentLevel].levelName;
        UIController
            .UpdateTextUI(UIController.UITextComponents.titleText,
            currentTitle,
            false);

        //FINALS
        //final test screenshot shows the end screen (downtime)
        //finalise sounds - add piano samples not audio helm (can be done in downtime)
        //build out 3/4 - get the backing tracks (downtime)
        //check back the note index being ++ because of resetting incorrect notes - is this correcT? needs BIG tests! (in downtime)
        //THIS
        //apply to midi help screen flash card and midi setup to game then finalise flash card tests
        //IF TIME
        //flats/sharps (use the game logic)
        //calculate all ypositions in logic using .11 as diff between notes (this may change if you change size of staff etc)
        //add a more faded colour on the non notes part of staff
        //add eighth notes properly
        //persist user index (accross all games/tests)
    }

    void Update()
    {
        if (StateController.currentState == StateController.States.end)
        {
            foreach (NotePrefab playedNote in playedNotes)
            playedNote.gameObject.SetActive(true);
        }
    }

    private void PlayNote(int note, float vel)
    {
        if (StateController.currentState == StateController.States.play)
        {
            if (note == allNotes[noteIndex + 1].note && CorrectPosition())
            {
                if (!allNotes[noteIndex + 1].guessedCorrectly)
                {
                    ScoreOnDistance();
                    ScoreController.totalCorrect++;
                    SpawnNote(note, true);
                }
                allNotes[noteIndex + 1].guessedCorrectly = true;
            }
            else
            {
                ScoreController.totalIncorrect++;
                incorrectPerNote++;
                SpawnNote(note, false);
            }
        }
    }

    private bool CorrectPosition()
    {
        //hard coding numbers - why is it .6f?
        float timeLinePosX = timeLine.transform.position.x;
        float currentNotePosX = spawedNotes[noteIndex + 1].localPosition.x;

        if (
            timeLinePosX >= currentNotePosX &&
            timeLinePosX < currentNotePosX + .6f
        )
        {
            return true;
        }
        else
            return false;
    }

    private float ScoreOnDistance()
    {
        var vectorToTarget =
            spawedNotes[noteIndex + 1].position - timeLine.transform.position;
        vectorToTarget.y = 0;
        var distanceToTarget = vectorToTarget.magnitude;
        int notesMistakes = allNotes[noteIndex + 1].incorretGuessesMade;
        if (notesMistakes == 0) notesMistakes = 1;
        if (distanceToTarget < CoreElements.i.perfectDist)
        {
            allNotes[noteIndex + 1].speed = "Perfect";
            ScoreController.AddScore(10 / notesMistakes);
            print("Perfect!");
        }
        else if (distanceToTarget < CoreElements.i.medDist)
        {
            allNotes[noteIndex + 1].speed = "Med";
            ScoreController.AddScore(5 / notesMistakes);
            print("Med!");
        }
        else
        {
            allNotes[noteIndex + 1].speed = "Slow";
            ScoreController.AddScore(1 / notesMistakes);
            print("Slow!");
        }
        return distanceToTarget;
    }

    private void NoteOff(int note)
    {
    }

    private void SpawnNote(int note, bool wasCorrect)
    {
        //Spawn note and set text values
        NotePrefab notePrefab = Instantiate(_notePrefab);
        Transform newParent =
            spawedNotes[noteIndex].GetComponent<NotePrefab>()._parent;
        notePrefab.transform.SetParent(newParent, false);
        notePrefab.SetNoteText(4, false);
        notePrefab.SetLedgerLine(note, true, wasCorrect);
        Color32 noteColour = wasCorrect ? Color.green : Color.red;
        notePrefab.SetTextColour (noteColour);

        //Set Position
        float posX = timeLine.transform.position.x;
        int index = note % 12;
        if (note > 71) index += 12;
        float posY = notePrefab.ySpawns[index];

        if (note < 60)
        {
            //using bass note
            posY -= 1.783f;
            if (note < 48) posY -= 0.68f;
        }

        notePrefab.transform.localPosition = new Vector3(posX - .2f, posY, 0);

        playedNotes.Add (notePrefab);
        notePrefab.gameObject.SetActive(false);
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
            notePrefab._parent = staffParent;
            notePrefab.SetNoteText(note.noteLength, note.isEighth);
            notePrefab.SetLedgerLine(note.note, false, false);
            int index = note.note % 12;
            if (note.note > 71) index += 12;
            float posY = notePrefab.ySpawns[index] - staffYDist;

            if (note.note < 60)
            {
                //using bass note
                foreach (Transform child in staffParent)
                {
                    if (child.tag == "Break") child.gameObject.SetActive(false);
                }
                posY -= 1.783f;
                if (note.note < 48) posY -= 0.68f;
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

            spawedNotes.Add(notePrefab.transform);
        }
    }
}
