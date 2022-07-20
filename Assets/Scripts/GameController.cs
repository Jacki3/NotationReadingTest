using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private NotesController notesController;

    [SerializeField]
    private RhythmLine rhythmLine;

    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    private int countDownTime;

    [SerializeField]
    private TextMeshProUGUI countDownTimeText;

    void Start()
    {
        StateController.currentState = StateController.States.countdown;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) StartCoroutine(DelayStart());

        if (StateController.currentState == StateController.States.end)
        {
            countDownTimeText.enabled = true;
            countDownTimeText.text = "end";
        }
    }

    IEnumerator DelayStart()
    {
        int timeWaited = 0;
        while (true)
        {
            timeWaited++;
            int timeWaitedText = (countDownTime + 1) - timeWaited;
            countDownTimeText.text = timeWaitedText.ToString();
            if (timeWaited >= countDownTime)
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

    private void StartTest()
    {
        StateController.currentState = StateController.States.play;
        audioController.PlayMusic();
        rhythmLine.StartMoving();
    }
}
