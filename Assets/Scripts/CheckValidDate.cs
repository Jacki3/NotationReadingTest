using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class CheckValidDate : MonoBehaviour
{
    public string fileName;

    public float waitTime;

    public GameObject errorScreen;

    private void Start()
    {
        StartCoroutine(CheckDate());
        errorScreen.SetActive(false);
    }

    private bool CheckValidity()
    {
        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");

        string downloadLocation =
            Application.persistentDataPath + "/" + fileName;
        ftpClient.download("Study4/endDate.txt", @downloadLocation);

        StreamReader sr =
            new StreamReader(Application.persistentDataPath + "/" + fileName);
        string contents = sr.ReadToEnd();
        sr.Close();

        System.DateTime endDate = System.DateTime.Parse(contents);
        DateTime today = DateTime.Today;

        if (endDate >= today)
        {
            File.Delete(Application.persistentDataPath + "/" + fileName);
            return true;
        }
        else
        {
            File.Delete(Application.persistentDataPath + "/" + fileName);
            errorScreen.SetActive(true);
            return false;
        }
    }

    IEnumerator CheckDate()
    {
        while (true)
        {
            if (InternetConnectionChecker.hasInternet)
            {
                CheckValidity();
                break;
            }
            else
                yield return new WaitForSeconds(waitTime);
        }
    }
}
