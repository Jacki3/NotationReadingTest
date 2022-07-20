using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotePrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro ledgerText;

    private TextMeshPro noteText;

    public float[] ySpawns;

    void Awake()
    {
        noteText = GetComponent<TextMeshPro>();
    }

    public void SetNoteText(int noteLength, bool isEighth)
    {
        ledgerText.characterSpacing = 0;

        switch (noteLength)
        {
            case 1:
                noteText.text = "q";
                break;
            case 2:
                noteText.text = "h";
                break;
            case 4:
                noteText.text = "w";
                ledgerText.characterSpacing = 12;
                break;
        }

        if (isEighth) noteText.text = "n";
    }

    public void SetLedgerLine(int note)
    {
        note %= 12;
        switch (note)
        {
            case 0:
                ledgerText.text = "__";
                break;
        }
    }
}
