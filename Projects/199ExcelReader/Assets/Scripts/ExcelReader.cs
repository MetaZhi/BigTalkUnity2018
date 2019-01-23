using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;
using UnityEngine.UI;

public class ExcelReader : MonoBehaviour
{
    void Start()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, "npcs.xlsx");
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                var collection = result.Tables[0].Rows;
                string names = string.Empty;
                foreach (DataRow item in collection)
                {
                     names += item[1];
                }
                GetComponent<Text>().text = names;

                var dataset = reader.AsDataSet();
                // 获取所有sheet
                var sheets = result.Tables;
                // 获取第一个sheet
                var sheet1 = result.Tables[0];

                // 获取所有行
                var rows = sheet1.Rows;
                // 总行数
                var rowCount = rows.Count;
            }
        }
    }
}
