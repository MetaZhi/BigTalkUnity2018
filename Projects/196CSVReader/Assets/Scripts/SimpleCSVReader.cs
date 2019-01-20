using System.IO;
using UnityEngine;

public class SimpleCSVReader : MonoBehaviour
{
    void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "data.csv");
        // 按行读取
        var csvStr = File.ReadAllLines(path);
        foreach (var item in csvStr)
        {
            var values = item.Split(',');
        }
    }
}
