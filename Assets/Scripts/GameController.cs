using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private NotesController notesController;

    [SerializeField]
    private RhythmLine rhythmLine;

    [SerializeField]
    private Transform timeLine;

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

    [SerializeField]
    private int levelsNeededToFinishTest;

    [SerializeField]
    private AudioSource countDownSource;

    [SerializeField]
    private AudioClip[] countDownClips;

    public static int level = 0;

    private bool helpShown;

    void Start()
    {
        StateController.currentState = StateController.States.countdown;
        level = (PlayerPrefs.GetInt("LevelsComplete"));

        int UILevel =
            level >= levelsNeededToFinishTest
                ? levelsNeededToFinishTest
                : level + 1;

        UIController
            .UpdateTextUI(UIController.UITextComponents.levelsCompleteText,
            "Exercises Completed: " + UILevel + "/" + levelsNeededToFinishTest,
            false);
    }

    void Update()
    {
        int totalLevelsComplete = (PlayerPrefs.GetInt("LevelsComplete"));
        if (totalLevelsComplete == levelsNeededToFinishTest)
        {
            StateController.currentState = StateController.States.testComplete;
        }

        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        if (StateController.currentState != StateController.States.testComplete)
        {
            if (StateController.currentState == StateController.States.end)
            {
                countDownTimeText.enabled = true;
                countDownTimeText.text = "end";
                lowBanner.SetActive(true);
                playButton.GetComponentInChildren<TextMeshProUGUI>().text =
                    "Next";
            }
        }
        else
        {
            UIController.ShowTestComplete();
        }
    }

    public void Initialise()
    {
        if (StateController.currentState != StateController.States.testComplete)
        {
            if (UIController.UserIndexFilled())
            {
                if (
                    StateController.currentState ==
                    StateController.States.countdown
                )
                {
                    notesController.SpawnNotes();
                    lowBanner.SetActive(false);
                    StartCoroutine(ReadCountDown());
                }
                if (StateController.currentState == StateController.States.end)
                {
                    SceneManager
                        .LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
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
        int countIndex = 0;
        while (true)
        {
            timeWaited++;
            countDownSource.PlayOneShot(countDownClips[countIndex]);
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
            countIndex++;
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

    public void ShowHelpScreen()
    {
        if (!helpShown)
        {
            helpShown = true;
            CoreElements.i.helpScreen.SetActive(true);
            UIController
                .UpdateTextUI(UIController.UITextComponents.helpButtonText,
                "Back",
                false);
            UIController
                .UpdateTextUI(UIController.UITextComponents.titleText,
                "Help Screen",
                false);
        }
        else
        {
            helpShown = false;
            CoreElements.i.helpScreen.SetActive(false);
            UIController
                .UpdateTextUI(UIController.UITextComponents.helpButtonText,
                "Help",
                false);
            string currentTitle = notesController.noteLevels[level].levelName;
            UIController
                .UpdateTextUI(UIController.UITextComponents.titleText,
                currentTitle,
                false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
