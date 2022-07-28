using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIController
{
    public enum UITextComponents
    {
        levelsCompleteText,
        warningText
    }

    public static void UpdateTextUI(
        UITextComponents component,
        string text,
        bool append
    )
    {
        if (CoreElements.i != null)
        {
            if (!append)
                CoreElements.i.GetTextComponent(component).text = text;
            else
                CoreElements.i.GetTextComponent(component).text += text;
        }
    }

    public static bool UserIndexFilled()
    {
        var indexField = CoreElements.i.indexField;
        if (indexField.text.Length == indexField.characterLimit)
        {
            UpdateTextUI(UITextComponents.warningText, "", false);
            CoreElements.i.userIndex = indexField.text;
            return true;
        }
        else
        {
            UpdateTextUI(UITextComponents.warningText,
            "Please fill out user index",
            false);
            return false;
        }
    }

    public static void ShowTestComplete()
    {
        CoreElements.i.testCompleteScreen.SetActive(true);
    }
}
