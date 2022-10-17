using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InternetConnectionChecker : MonoBehaviour
{
    public float checkTime = 1;

    public GameObject warningScreen;

    public static bool hasInternet;

    private void Start()
    {
        StartCoroutine(CheckConnection());
    }

    IEnumerator CheckConnection()
    {
        while (true)
        {
            UnityWebRequest newRequest =
                new UnityWebRequest("https://www.google.com/"); //replace with your own server
            yield return newRequest.SendWebRequest();

            if (newRequest.error == null)
            {
                warningScreen.SetActive(false);
                hasInternet = true;
            }
            else
            {
                warningScreen.SetActive(true);
                hasInternet = false;
                if (StateController.currentState == StateController.States.play)
                {
                    SceneManager
                        .LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }

            yield return new WaitForSeconds(checkTime);
        }
    }
}
