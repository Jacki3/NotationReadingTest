using TMPro;
using UnityEngine;

public class NotePrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro ledgerText;

    [SerializeField]
    private int totalYSpawns = 48;

    [SerializeField]
    private float startingYPos = -1.65f;

    [SerializeField]
    private float yDiff = 1f;

    [SerializeField]
    private TextMeshPro upsideNote;

    private TextMeshPro noteText;

    public Transform _parent;

    public float[] ySpawns;

    void Awake()
    {
        noteText = GetComponent<TextMeshPro>();

        ySpawns = new float[totalYSpawns];
        int index = 0;
        for (int i = 0; i < totalYSpawns; i++)
        {
            ySpawns[index] = startingYPos;
            if (
                index == 0 ||
                index == 12 ||
                index == 24 ||
                index == 36 ||
                index == 48 ||
                index % 4 != 0 &&
                index != 11 &&
                index != 23 &&
                index != 35 &&
                index != 47
            )
            {
                if (index < ySpawns.Length - 2)
                {
                    ySpawns[index + 1] = startingYPos;
                    index += 2;
                    startingYPos += yDiff;
                }
            }
            else
            {
                if (index < ySpawns.Length - 1)
                {
                    index++;
                    startingYPos += yDiff;
                }
            }
        }
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

    public void SetLedgerLine(int note, bool isPlayedNote, bool correctGuess)
    {
        if (note >= 71)
        {
            upsideNote.text = noteText.text;
            noteText.text = "";
        }
        if (note < 60 && note > 48)
        {
            upsideNote.text = noteText.text;
            noteText.text = "";
        }
        switch (note)
        {
            case 60:
                ledgerText.text = "__";
                break;
            case 40:
                ledgerText.text = "__";
                break;
            case 81:
                ledgerText.text = "__";
                break;
        }

        if (isPlayedNote)
        {
            if (correctGuess)
            {
                ledgerText.color = Color.green;
                upsideNote.color = Color.green;
            }
            else
            {
                ledgerText.color = Color.red;
                upsideNote.color = Color.red;
            }
        }
    }

    public void SetTextColour(Color32 color)
    {
        noteText.color = color;
    }
}
