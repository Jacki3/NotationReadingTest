using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    float defaultXPos;

    void Start()
    {
        defaultXPos = transform.position.x;
    }

    public void StartTimeLine()
    {
        StartCoroutine(MoveToTime());
    }

    void Update()
    {
        //using a hard coded number!
        if (transform.position.x >= 8.88f)
        {
            transform.position =
                new Vector3(defaultXPos,
                    transform.position.y - NotesController.distanceY,
                    transform.position.z);
            StartCoroutine(MoveToTime());
        }
    }

    // 8.88 - end of the bar
    public IEnumerator MoveToTime()
    {
        Vector3 endBar =
            new Vector3(8.88f, transform.position.y, transform.position.z);
        Vector3 currentPos = transform.position;

        float elapsedTime = 0;
        float waitTime = AudioController.beatPerSec * 16; //How long a beat takes multiplied by the total beats possible in a single staff (4 bars)
        while (elapsedTime < waitTime)
        {
            transform.position =
                Vector3.Lerp(currentPos, endBar, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endBar;
        yield return null;
    }
}
