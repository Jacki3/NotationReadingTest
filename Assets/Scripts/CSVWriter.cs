using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    [HideInInspector]
    public string testState = "preTest";

    private string wholeFilePath;

    private string fileName;

    private string screenGrabName;

    private string wholeFilePathScreen;

    public void WriteCSV(string data)
    {
        fileName =
            CoreElements.i.userIndex +
            "_NoteReadingTest_" +
            testState +
            "_" +
            GameController.level +
            "_Results_" +
            System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
            ".csv";
        wholeFilePath = Application.dataPath + "/" + fileName;

        StreamWriter tw = new StreamWriter(wholeFilePath, true);

        string newData = "";
        newData += data;

        tw.Write (newData);

        tw.Close();
    }

    public void UploadResults()
    {
        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");

        // ftpClient
        //     .createDirectory("/Study4/NoteReadingResults/" +
        //     CoreElements.i.userIndex.ToString());
        ftpClient
            .upload("Study4/NoteReadingResults/" +
            CoreElements.i.userIndex.ToString() +
            "/" +
            fileName,
            @wholeFilePath);
    }

    public void ScreenGrab()
    {
        screenGrabName =
            CoreElements.i.userIndex +
            "_NoteReadingTest_" +
            testState +
            "_" +
            GameController.level +
            "_Screenshot_" +
            System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
            ".png";

        wholeFilePathScreen = Application.dataPath + "/" + screenGrabName;
        ScreenCapture.CaptureScreenshot((wholeFilePathScreen), 2);

        Invoke("UploadDelay", .75f);
    }

    private void UploadDelay()
    {
        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");
        ftpClient
            .UploadImage(@wholeFilePathScreen,
            "Study4/NoteReadingResults/" +
            CoreElements.i.userIndex.ToString() +
            "/" +
            screenGrabName);
    }
}
