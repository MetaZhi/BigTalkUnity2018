using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

    public GameObject RankItemTemplate;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < 100; i++)
        {
            var go = Instantiate(RankItemTemplate, RankItemTemplate.transform.parent);
            go.SetActive(true);

            var rankItem = go.GetComponent<RankItem>();
            rankItem.SetInfo(i.ToString(), i);

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() => OnClickItem(go));
        }		
	}

    void OnClickItem(GameObject go)
    {
        var item = go.GetComponent<RankItem>();

        Debug.Log(item.Name + item.Score);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
