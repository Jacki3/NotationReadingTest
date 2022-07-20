using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float durationOfMove = .5f;

    public void MoveCameraDown()
    {
        StartCoroutine(LerpPosition());
    }

    IEnumerator LerpPosition()
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        float targetY = transform.position.y - NotesController.distanceY * 2;
        Vector2 targetPosition = new Vector2(transform.position.x, targetY);

        while (time < durationOfMove)
        {
            transform.position =
                Vector2
                    .Lerp(startPosition, targetPosition, time / durationOfMove);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
