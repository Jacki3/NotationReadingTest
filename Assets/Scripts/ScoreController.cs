using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class ScoreController
{
    public static int totalScore;

    static void OnEnable()
    {
        SceneManager.sceneLoaded += ResetStaticVariables;
    }

    static void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetStaticVariables;
    }

    private static void ResetStaticVariables(Scene scene, LoadSceneMode mode)
    {
        totalScore = 0;
    }

    public static void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
    }
}
