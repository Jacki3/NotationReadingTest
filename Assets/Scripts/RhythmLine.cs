using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmLine : MonoBehaviour
{
    [SerializeField]
    private float distanceToMove = 1;

    [SerializeField]
    private CameraMovement cameraMovement;

    private float defaultXPos;

    private int movesMade = 0;

    private int movesMadeOnStaff = 0;

    public int totalMovesMade = 0;

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
        float dist = distanceToMove;
        float yPos = transform.position.y;
        yield return new WaitForSeconds(AudioController.beatPerSec);
        while (true)
        {
            movesMade++;
            movesMadeOnStaff++;
            totalMovesMade++;
            allMoves++;

            // if (movesMade >= 4)
            // {
            //     dist = distanceToMove + .5f;
            //     movesMade = 0;
            // }
            // else
            //     dist = distanceToMove;
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

            if ((totalMovesMade % 32) == 0)
            {
                print("moving cam");
                cameraMovement.MoveCameraDown();
            }

            transform.position += new Vector3(dist, 0, 0);
            yield return new WaitForSeconds(AudioController.beatPerSec);

            if (allMoves == NotesController.totalNotes)
            {
                StateController.currentState = StateController.States.end;
                break;
            }
        }
    }
}
