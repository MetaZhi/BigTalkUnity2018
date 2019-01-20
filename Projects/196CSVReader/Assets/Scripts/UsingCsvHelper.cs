using CsvHelper;
using System.IO;
using System.Linq;
using UnityEngine;

public class Npc
{
    public int Id { get; set; }
    public string 名称 { get; set; }
    public float 血量 { get; set; }
    public float 攻击力 { get; set; }
    public float 防御力 { get; set; }
}

public class UsingCsvHelper : MonoBehaviour
{
    void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "data.csv");
        var csvStr = File.ReadAllText(path);

        using (var reader = new StringReader(csvStr))
        using (var csv = new CsvReader(reader))
        {
            // 需要using System.Linq;才能使用ToList()
            var records = csv.GetRecords<Npc>().ToList();
            Debug.Log(records.Count);
        }
    }
}
