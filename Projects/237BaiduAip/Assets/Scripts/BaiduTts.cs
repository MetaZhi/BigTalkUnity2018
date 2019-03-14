using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaiduTts : MonoBehaviour
{
    public string APIKey;
    public string SecretKey;
    public string Text = "洪流学堂，让你快人几步";
    private string Token;
    private AudioClip _clipRecord;

    // 用于解析返回的json
    [Serializable]
    class TokenResponse
    {
        public string access_token = null;
    }

    [Serializable]
    public class AsrResponse
    {
        public int err_no;
        public string err_msg;
        public string sn;
        public string[] result;
    }

    IEnumerator Start()
    {
        // 拼接请求的URL
        var uri = $"https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={APIKey}&client_secret={SecretKey}";
        var www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError("[BaiduAip]" + www.error);
            Debug.LogError("[BaiduAip]Token was fetched failed. Please check your APIKey and SecretKey");
        }
        else
        {            
            Debug.Log("[BaiduAip]" + www.downloadHandler.text);
            var result = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
            Token = result.access_token;
            Debug.Log("[WitBaiduAip]Token has been fetched successfully");
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("[WitBaiduAip demo]开始合成");
            StartCoroutine(Tts(Text, s =>
            {
                var text = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";

                Debug.Log(text);
            }));
        }
    }

    public IEnumerator Tts(string text, Action<AsrResponse> callback)
    {
        var uri = $"http://tsn.baidu.com/text2audio";

        var param = new Dictionary<string, string>();
            param.Add("tex", text);
            param.Add("tok", Token);
            param.Add("cuid", SystemInfo.deviceUniqueIdentifier);
            param.Add("ctp", "1");
            param.Add("lan", "zh");
            param.Add("spd", "5");
            param.Add("pit", "5");
            param.Add("vol", "10");
            param.Add("per", "1");
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
            param.Add("aue", "6"); // set to wav, default is mp3
#endif

        string data = "";
        int i = 0;
        foreach (var p in param)
        {
            data += i != 0 ? "&" : "?";
            data += p.Key + "=" + p.Value;
            i++;
        }
        
        var www = UnityWebRequest.Post(uri, data);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("[WitBaiduAip]" + www.downloadHandler.text);
            callback(JsonUtility.FromJson<AsrResponse>(www.downloadHandler.text));
        }
        else
            Debug.LogError(www.error);
    }
}
