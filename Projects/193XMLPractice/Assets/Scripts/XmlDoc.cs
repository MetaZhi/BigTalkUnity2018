using UnityEngine;
// ע��ʹ��Xml��Ҫ������������ռ�
using System.Xml;
using System.IO;

public class XmlDoc : MonoBehaviour
{
    void Start()
    {
        // ʹ��Path.Combine����·��ƴ�ӵĴ���
        var path = Path.Combine(Application.streamingAssetsPath, "npcs.xml");
        // ����XmlDocument����
        XmlDocument doc = new XmlDocument();
        // �����ļ�
        doc.Load(path);

        //ѡ��npcs����ڵ�
        var npcs = doc.SelectSingleNode("/npcs");

        //ͨ��ѭ����ȡÿ��npc�ڵ��name�ӽڵ㣬��InnerText��ȡ����
        foreach (XmlNode npc in npcs.ChildNodes)
        {
            Debug.Log(npc.SelectSingleNode("name").InnerText);
        }
    }
}
