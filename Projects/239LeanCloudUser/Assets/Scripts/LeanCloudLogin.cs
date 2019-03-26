using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeanCloudLogin : MonoBehaviour
{
    public string AppId;
    public string AppKey;

    public InputField Username;
    public InputField Password;

    // Update is called once per frame
    public void Login()
    {
        StartCoroutine(LoginCo());
    }

    IEnumerator LoginCo(){
        
        var jsonObj = new RegJson()
        {
            username = Username.text,
            password = Password.text
        };

        // 从文档获取的url
        var url = "https://5jmvfx9e.api.lncld.net/1.1/login";

        var json = JsonUtility.ToJson(jsonObj);
        Debug.Log(json);

        // 采用昨天大智讲的取巧的办法POST json数据
        var www = UnityWebRequest.Put(url, json);
        www.method = "POST";
        www.SetRequestHeader("X-LC-Id", AppId);
        www.SetRequestHeader("X-LC-Key", AppKey);
        www.SetRequestHeader("Content-Type", "application/json");


        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError(www.error);
            Debug.LogError(www.downloadHandler.text);
        }
        else{
            Debug.Log(www.downloadHandler.text);
        }
    }
}
