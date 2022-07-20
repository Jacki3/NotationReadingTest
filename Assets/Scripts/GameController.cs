using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private NotesController notesController;

    [SerializeField]
    private RhythmLine rhythmLine;

    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    private int countInTime;

    [SerializeField]
    private int readTime;

    [SerializeField]
    private TextMeshProUGUI countDownTimeText;

    [SerializeField]
    private GameObject lowBanner;

    [SerializeField]
    private Button playButton;

    void Start()
    {
        StateController.currentState = StateController.States.countdown;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) Initialise();
        if (StateController.currentState == StateController.States.end)
        {
            countDownTimeText.enabled = true;
            countDownTimeText.text = "end";
            lowBanner.SetActive(true);
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        }
    }

    public void Initialise()
    {
        if (StateController.currentState == StateController.States.countdown)
        {
            notesController.SpawnNotes();
            lowBanner.SetActive(false);
            StartCoroutine(ReadCountDown());
        }
        if (StateController.currentState == StateController.States.end)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    IEnumerator ReadCountDown()
    {
        int timeWaited = 0;
        while (true)
        {
            timeWaited++;
            int timeWaitedText = (readTime + 1) - timeWaited;
            countDownTimeText.text = timeWaitedText.ToString();
            if (timeWaited >= readTime)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(DelayStart());
                StartCoroutine(FlashLine());
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator DelayStart()
    {
        int timeWaited = 0;
        while (true)
        {
            timeWaited++;
            int timeWaitedText = (countInTime + 1) - timeWaited;
            countDownTimeText.text = timeWaitedText.ToString();
            if (timeWaited >= countInTime)
            {
                yield return new WaitForSeconds(AudioController.beatPerSec);
                countDownTimeText.text = "go";
                countDownTimeText.enabled = false;
                StartTest();
                break;
            }
            yield return new WaitForSeconds(AudioController.beatPerSec);
        }
    }

    IEnumerator FlashLine()
    {
        while (true)
        {
            if (StateController.currentState == StateController.States.countdown
            )
            {
                rhythmLine.gameObject.SetActive(false);
                yield return new WaitForSeconds(AudioController.beatPerSec / 2);
                rhythmLine.gameObject.SetActive(true);
                yield return new WaitForSeconds(AudioController.beatPerSec / 2);
            }
            else
                break;
        }
    }

    private void StartTest()
    {
        StateController.currentState = StateController.States.play;
        audioController.PlayMusic();
        rhythmLine.StartMoving();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
