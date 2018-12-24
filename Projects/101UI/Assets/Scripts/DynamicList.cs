using System.Collections.Generic;
using UnityEngine;

public class DynamicList : MonoBehaviour
{

    public GameObject ItemPrefab;
    public Transform Content;

    List<GameObject> Items = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            // var go = Instantiate(ItemPrefab, Content);
            var go = Instantiate(ItemPrefab);
            go.transform.SetParent(Content, false);



            // 通常你需要将生成的列表项保存下来，因为很多时候还需要删除它
            Items.Add(go);
        }
    }
}
