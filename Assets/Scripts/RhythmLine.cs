using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int allMoves = 0;

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
        StartCoroutine(timeLine.MoveToTime());
        float dist = distanceToMove;
        float yPos = transform.position.y;
        yield return new WaitForSeconds(AudioController.beatPerSec);
        while (true)
        {
            const float interval = 1f;
            float nextEventTime = Time.time + interval;
            if (Time.time >= nextEventTime)
            {
                nextEventTime += interval;
            }
            movesMade++;
            movesMadeOnStaff++;
            totalMovesMade++;
            allMoves++;

            if (movesMadeOnStaff >= 16)
            {
                movesMadeOnStaff = 0;
                dist = distanceToMove;
                yPos -= NotesController.distanceY;
                transform.position = new Vector3(defaultXPos, yPos, .5f);
                movesMade++;
                totalMovesMade++;
                movesMadeOnStaff++;
                yield return new WaitForSeconds(AudioController.beatPerSec);
            }

            if ((totalMovesMade % 47) == 0)
            {
                cameraMovement.MoveCameraDown();
            }

            transform.position += new Vector3(dist, 0, 0);
            yield return new WaitForSeconds(AudioController.beatPerSec);

            if (
                allMoves - 1 ==
                NotesController.totalNotes - NotesController.totalStaffs_Static
            )
            {
                StateController.currentState = StateController.States.end;
                break;
            }

            yield return null;
        }
    }
}
