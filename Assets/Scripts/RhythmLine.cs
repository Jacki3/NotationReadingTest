using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class RhythmLine : MonoBehaviour
{
    [SerializeField]
    private float distanceToMove = 1;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private TimeLine timeLine;

    private float defaultXPos;

    private int movesMade = 0;

    private int movesMadeOnStaff = 0;

    private int totalMovesMade = 0;

    public int allMoves = 0;

    void Start()
    {
        defaultXPos = transform.position.x;
        print (defaultXPos);
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

        bool movedDown = false;

        while (this)
        {
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
                    GameController.level++;
                    PlayerPrefs.SetInt("LevelsComplete", GameController.level);
                    StateController.currentState = StateController.States.end;
                    break;
                }
                print (movesMadeOnStaff);
            }

            if (time >= nextEventTimeLine)
            {
                if (movedDown)
                {
                    nextEventTimeLine += intervalLine;
                    timeLine.transform.position = new Vector3(-6.761f, yPos, 0); //using hard code here
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
