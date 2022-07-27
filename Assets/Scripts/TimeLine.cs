using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private float defaultXPos;

    void Start()
    {
        defaultXPos = transform.position.x;
    }

    void Update()
    {
        //using a hard coded number!
        // 8.9 - end of the bar
        // if (transform.position.x >= 8.9f)
        // {
        //     transform.position =
        //         new Vector3(defaultXPos,
        //             transform.position.y - NotesController.distanceY,
        //             transform.position.z);
        // }
    }
}
