using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateController
{
    public static States currentState;

    public enum States
    {
        countdown,
        play,
        end
    }
}
