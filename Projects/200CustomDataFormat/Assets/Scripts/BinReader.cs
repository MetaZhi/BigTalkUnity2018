using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

public class BinReader : MonoBehaviour
{
    void Start()
    {
        var binPath = Path.Combine(Application.streamingAssetsPath, "npcs.bin");

        using (var binStream = File.OpenRead(binPath))
        using (var binReader = new BinaryReader(binStream))
        {
            try
            {
                while (true)
                {
                    Debug.Log(binReader.ReadByte());
                    Debug.Log(binReader.ReadString());
                    Debug.Log(binReader.ReadUInt32());
                    Debug.Log(binReader.ReadUInt16());
                    Debug.Log(binReader.ReadUInt16());
                }
            }
            catch (EndOfStreamException)
            {
                return;
            }
        }
    }
}
