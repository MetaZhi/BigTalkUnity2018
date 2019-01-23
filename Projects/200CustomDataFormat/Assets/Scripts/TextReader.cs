using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextReader : MonoBehaviour
{
    void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "config.ini");
        // 按行读出来
        var text = File.ReadAllLines(path);
        foreach (var t in text)
        {
            var kv = t.Split('=');
            Debug.Log($"Key-Value:{kv[0]}={kv[1]}");
        }
    }
}
