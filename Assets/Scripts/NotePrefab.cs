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

    public void SetNoteText(int noteLength)
    {
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
                break;
        }
    }

    public void SetLedgerLine(int note)
    {
        switch (note)
        {
            case 0:
                ledgerText.text = "__";
                break;
        }
    }
}
