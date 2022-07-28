using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private string wholeFilePath;

    private string fileName;

    public void WriteCSV(string data)
    {
        fileName =
            CoreElements.i.userIndex +
            "_NoteReadingTest_Test_" +
            GameController.level +
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
        ftpClient
            .createDirectory("/Study4/NoteReadingResults/" +
            CoreElements.i.userIndex.ToString());
        ftpClient
            .upload("/Study4/NoteReadingResults/" +
            CoreElements.i.userIndex.ToString() +
            "/" +
            fileName,
            @wholeFilePath);
    }
}
