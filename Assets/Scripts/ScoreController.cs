using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreController
{
    public static int totalScore;

    public static void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
    }
}
