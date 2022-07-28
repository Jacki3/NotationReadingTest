using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoreElements : MonoBehaviour
{
    public TMP_InputField indexField;

    public string userIndex;

    public GameObject testCompleteScreen;

    public UIText[] textComponents;

    [System.Serializable]
    public class UIText
    {
        public static TextMeshProUGUI textMeshProUGUI;

        public UIController.UITextComponents textComponent;

        public TextMeshProUGUI textPlaceholder;
    }

    private static CoreElements _i;

    public static CoreElements i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;
    }

    public TextMeshProUGUI
    GetTextComponent(UIController.UITextComponents textComponent)
    {
        foreach (UIText text in textComponents)
        {
            if (text.textComponent == textComponent)
                return text.textPlaceholder;
        }
        Debug.LogError("Text Component" + textComponent + "missing!");
        return null;
    }
}
