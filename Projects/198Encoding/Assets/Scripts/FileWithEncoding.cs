using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FileWithEncoding : MonoBehaviour
{
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        var path = Path.Combine(Application.streamingAssetsPath, "data.txt");
        Debug.Log(Encoding.Default);
        var t = File.ReadAllText(path);
        text.text = t;
    }
}
