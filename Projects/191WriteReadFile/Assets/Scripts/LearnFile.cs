using System.IO;
using UnityEngine;

public class LearnFile : MonoBehaviour
{
    void Start()
    {
        var str = File.ReadAllText("text.txt");
        Debug.Log(str);
    }
}
