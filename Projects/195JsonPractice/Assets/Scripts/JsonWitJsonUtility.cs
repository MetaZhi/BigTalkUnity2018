using System.IO;
using UnityEngine;

[System.Serializable]
public class Npc
{
    public int Id;
    public string 名称;
    public float 血量;
    public float 攻击力;
    public float 防御力;
}

public class Npcs
{
    public Npc[] npcs;
}


public class JsonWitJsonUtility : MonoBehaviour
{
    void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "dataobject.json");
        var jsonStr = File.ReadAllText(path);

        var npcs = JsonUtility.FromJson<Npcs>(jsonStr);
        Debug.Log(npcs.npcs.Length);
    }
}
