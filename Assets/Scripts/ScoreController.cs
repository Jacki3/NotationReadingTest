using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class ScoreController
{
    public static float totalScore;

    public static int totalCorrect;

    public static int totalIncorrect;

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
        totalCorrect = 0;
        totalIncorrect = 0;
    }

    public static void AddScore(float scoreToAdd)
    {
        totalScore += scoreToAdd;
    }
}
