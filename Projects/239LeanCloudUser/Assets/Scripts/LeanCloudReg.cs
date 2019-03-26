using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
class RegJson
{
    public string username;
    public string password;
}

public class LeanCloudReg : MonoBehaviour
{
    public string AppId;
    public string AppKey;

    public InputField Username;
    public InputField Password;

    // Update is called once per frame
    public void Reg()
    {
        StartCoroutine(RegCo());
    }

    IEnumerator RegCo(){
        
        var jsonObj = new RegJson()
        {
            username = Username.text,
            password = Password.text
        };

        var url = "https://5jmvfx9e.api.lncld.net/1.1/users";

        var json = JsonUtility.ToJson(jsonObj);
        Debug.Log(json);
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

    IEnumerator RegCoLowAPI()
    {

        var jsonObj = new RegJson()
        {
            username = Username.text,
            password = Password.text
        };

        var url = "https://5jmvfx9e.api.lncld.net/1.1/users";

        var json = JsonUtility.ToJson(jsonObj);
        Debug.Log(json);

        var www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SetRequestHeader("X-LC-Id", AppId);
        www.SetRequestHeader("X-LC-Key", AppKey);
        www.SetRequestHeader("Content-Type", "application/json");


        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError(www.error);
            Debug.LogError(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
