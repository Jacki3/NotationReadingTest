using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class RhythmLine : MonoBehaviour
{
    [SerializeField]
    private float distanceToMove = 1;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private TimeLine timeLine;

    [SerializeField]
    private NotesController notesController;

    [SerializeField]
    private CSVWriter csvWriter;

    private float defaultXPos;

    private int movesMade = 0;

    private int movesMadeOnStaff = 0;

    private int totalMovesMade = 0;

    public int allMoves = 0;

    void Start()
    {
        defaultXPos = transform.position.x;
    }

    public void StartMoving()
    {
        StartCoroutine(MoveToBeat());
    }

    private IEnumerator MoveToBeat()
    {
        float dist = distanceToMove;
        float yPos = transform.position.y;
        double interval = AudioController.beatPerSecDouble;
        double time = AudioSettings.dspTime;
        double nextEventTime = time + interval;
        double intervalLine = interval / 10;
        double nextEventTimeLine = time + intervalLine;

        print(NotesController.totalNotes +
        1 -
        NotesController.totalStaffs_Static);

        bool movedDown = false;

        while (this)
        {
            if (
                notesController.noteIndex <
                notesController.spawedNotes.Count - 1
            )
            {
                float nextNotePosX =
                    notesController
                        .spawedNotes[notesController.noteIndex + 1]
                        .localPosition
                        .x;
                float timeLinePosX = timeLine.transform.position.x;
                float staffPosY =
                    notesController
                        .spawedNotes[notesController.noteIndex + 1]
                        .parent
                        .localPosition
                        .y;
                float timeLinePosY = timeLine.transform.position.y;
                if (timeLinePosX >= nextNotePosX && timeLinePosY == staffPosY)
                {
                    notesController.noteIndex++;
                }
            }

            time = AudioSettings.dspTime;
            if (time >= nextEventTime)
            {
                movesMade++;
                movesMadeOnStaff++;
                totalMovesMade++;
                allMoves++;
                if (movesMadeOnStaff >= 16)
                {
                    movedDown = true;
                    nextEventTime += interval;
                    movesMadeOnStaff = 0;
                    dist = distanceToMove;
                    yPos -= NotesController.distanceY;
                    transform.position = new Vector3(defaultXPos, yPos, 0.5f);

                    movesMade++;
                    totalMovesMade++;
                    movesMadeOnStaff++;
                }
                else
                {
                    nextEventTime += interval;
                    transform.position += new Vector3(dist, 0, 0);
                }
                if ((totalMovesMade % 47) == 0)
                {
                    cameraMovement.MoveCameraDown();
                }

                if (
                    allMoves - 1 ==
                    NotesController.totalNotes +
                    1 -
                    NotesController.totalStaffs_Static
                )
                {
                    csvWriter.WriteCSV(ScoreController.totalScore.ToString());
                    csvWriter.UploadResults();
                    GameController.level++;
                    PlayerPrefs.SetInt("LevelsComplete", GameController.level);
                    StateController.currentState = StateController.States.end;
                    break;
                }
            }

            if (time >= nextEventTimeLine)
            {
                if (movedDown)
                {
                    nextEventTimeLine += intervalLine;
                    timeLine.transform.position = new Vector3(-6.761f, yPos, 1); //using hard code here
                    movedDown = false;
                }
                else
                {
                    nextEventTimeLine += intervalLine;
                    timeLine.transform.position += new Vector3(.1f, 0, 0);
                }
            }
            yield return null;
        }
    }
}
