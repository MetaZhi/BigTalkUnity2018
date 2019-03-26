using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// 上传分数的Score类，用于序列化成json
[Serializable]
public class Score
{
    public string username;
    public string userId;
    public int score;
}

// 用于解析登陆后的token，这个token用来代表已登录的用户
[Serializable]
public class UserInfo{
    public string sessionToken;
    public string username;
    public string objectId;
}

public class LeanCloudUser : MonoBehaviour
{
    public string AppId;
    public string AppKey;

    public InputField Username;
    public InputField Password;

    private UserInfo User;

    public void Reg()
    {
        var jsonObj = new RegJson()
        {
            username = Username.text,
            password = Password.text
        };
        var json = JsonUtility.ToJson(jsonObj);
        Debug.Log(json);
        StartCoroutine(Request("/users", "POST", json));
    }

    public void Login()
    {
        var jsonObj = new RegJson()
        {
            username = Username.text,
            password = Password.text
        };
        var json = JsonUtility.ToJson(jsonObj);
        Debug.Log(json);
        StartCoroutine(Request("/login", "POST", json, text =>
        {
            var obj = JsonUtility.FromJson<UserInfo>(text);
            User = obj;

            UploadMyScore(Random.Range(1, 100));
        }));
    }

    // 重构了请求的类，现在可以更好地适用各种请求
    IEnumerator Request(string path, string method = "POST", string data = "", Action<string> cb = null)
    {

        var url = "https://5jmvfx9e.api.lncld.net/1.1" + path;

        var downloadHandler = new DownloadHandlerBuffer();
        UploadHandlerRaw uploadHandler = null;
        if (!string.IsNullOrEmpty(data))
         uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        var www = new UnityWebRequest(url, method, downloadHandler, uploadHandler);
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
            if (cb != null) cb(www.downloadHandler.text);
        }
    }

    // 上传分数的方法
    private void UploadMyScore(int score)
    {
        var json = JsonUtility.ToJson(new Score(){
            score = score,
            username = User.username,
            userId = User.objectId
        });

        StartCoroutine(Request("/classes/Score", "POST", json, _=>{
            GetLeaderboard();
        }));
    }

    public void GetLeaderboard(){
        var path = "/classes/Score?";
        path += "order=-score&limit=10";

        StartCoroutine(Request(path, "GET"));
    }
}
