using UnityEngine;
// 注意使用Xml需要引入这个命名空间
using System.Xml;
using System.IO;

public class XmlDoc : MonoBehaviour
{
    void Start()
    {
        // 使用Path.Combine减少路径拼接的错误
        var path = Path.Combine(Application.streamingAssetsPath, "npcs.xml");
        // 创建XmlDocument对象
        XmlDocument doc = new XmlDocument();
        // 加载文件
        doc.Load(path);

        //选择npcs这个节点
        var npcs = doc.SelectSingleNode("/npcs");

        //通过循环读取每个npc节点的name子节点，用InnerText获取数据
        foreach (XmlNode npc in npcs.ChildNodes)
        {
            Debug.Log(npc.SelectSingleNode("name").InnerText);
        }
    }
}
