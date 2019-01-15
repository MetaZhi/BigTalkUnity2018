using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[XmlType("npc")]
public class Npc
{
    public string name;
    public float hp;
    public float attack;
    public float def;
}

[XmlType("npcs")]
public class Npcs : List<Npc>
{

}

public class XmlDecode : MonoBehaviour
{
    void Start()
    {
        // 使用Path.Combine减少路径拼接的错误
        var path = Path.Combine(Application.streamingAssetsPath, "npcs.xml");
        var xmlStr = File.ReadAllText(path);

        // 使用using，在离开作用范围后会自动释放rdr
        using (var rdr = new StringReader(xmlStr))
        {
            //声明序列化对象实例serializer
            XmlSerializer serializer = new XmlSerializer(typeof(Npcs));
            //反序列化，并将反序列化结果值赋给变量i
            var npcs = (Npcs)serializer.Deserialize(rdr);
            Debug.Log(npcs.Count);
        }
    }
}
