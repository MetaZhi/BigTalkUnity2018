using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

public class BinWriter : MonoBehaviour
{
    [ContextMenu("ConvertToBinary")]
    void ConvertToBinary()
    {
        var excelPath = Path.Combine(Application.streamingAssetsPath, "npcs.xlsx");
        var binPath = Path.Combine(Application.streamingAssetsPath, "npcs.bin");

        using (var excel = File.Open(excelPath, FileMode.Open, FileAccess.Read))
        // 用流的方式打开要写入的二进制文件
        using (var binStream = File.OpenWrite(binPath))
        // 使用BinaryWriter写入文件
        using (var binWriter = new BinaryWriter(binStream))
        {
            using (var reader = ExcelReaderFactory.CreateReader(excel))
            {
                var result = reader.AsDataSet();

                var collection = result.Tables[0].Rows;
                Debug.Log(collection.Count);
                for (int i = 1; i < collection.Count; i++)
                {
                    DataRow item = collection[i];

                    // 根据写入数据的类型，写入对应的字节
                    binWriter.Write(byte.Parse(item[0].ToString()));
                    binWriter.Write(item[1] as string);
                    binWriter.Write(uint.Parse(item[2].ToString()));
                    binWriter.Write(ushort.Parse(item[3].ToString()));
                    binWriter.Write(ushort.Parse(item[4].ToString()));
                }
            }
        }
    }
}
