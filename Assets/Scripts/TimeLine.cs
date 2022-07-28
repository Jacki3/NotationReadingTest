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
}
