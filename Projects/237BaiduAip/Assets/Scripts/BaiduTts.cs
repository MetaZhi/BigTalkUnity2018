using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class BaiduTts : MonoBehaviour
{
    public string APIKey;
    public string SecretKey;
    public string Text = "洪流学堂，让你快人几步";
    private string Token;

    public AudioSource AudioSource { get; private set; }

    // 用于解析返回的json
    [Serializable]
    class TokenResponse
    {
        public string access_token = null;
    }

    /// <summary>
    ///     语音合成结果
    /// </summary>
    [Serializable]
    public class TtsResponse
    {
        public int err_no;
        public string err_msg;
        public string sn;
        public int idx;

        public bool Success
        {
            get { return err_no == 0; }
        }

        public AudioClip clip;
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


        AudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("[WitBaiduAip demo]开始合成");
            StartCoroutine(Tts(Text, s =>
            {
                AudioSource.clip = s.clip;
                AudioSource.Play();
            }));
        }
    }

    public IEnumerator Tts(string text, Action<TtsResponse> callback)
    {
        var url = "http://tsn.baidu.com/text2audio";

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
        param.Add("aue", "6"); //设置为wav格式，移动端需要mp3格式
#endif

        int i = 0;
        foreach (var p in param)
        {
            url += i != 0 ? "&" : "?";
            url += p.Key + "=" + p.Value;
            i++;
        }

        // 根据不同平台，获取不同类型的音频格式
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
        var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
#else
        var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
#endif
        Debug.Log("[WitBaiduAip]" + www.url);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
            Debug.LogError(www.error);
        else
        {
            var type = www.GetResponseHeader("Content-Type");
            Debug.Log("[WitBaiduAip]response type: " + type);

            if (type.Contains("audio"))
            {
                var response = new TtsResponse { clip = DownloadHandlerAudioClip.GetContent(www) };
                callback(response);
            }
            else
            {
                var textBytes = www.downloadHandler.data;
                var errorText = Encoding.UTF8.GetString(textBytes);
                Debug.LogError("[WitBaiduAip]" + errorText);
                callback(JsonUtility.FromJson<TtsResponse>(errorText));
            }
        }
    }
}
