using System.IO;
using UnityEngine;
using LitJson;

public class JsonWitLitJson : MonoBehaviour
{
    void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "data.json");
        var jsonStr = File.ReadAllText(path);

        // Npc类的定义在我们上一个代码中哦。
        var npcs = JsonMapper.ToObject<Npc[]>(jsonStr);
        Debug.Log(npcs.Length);
    }
}
