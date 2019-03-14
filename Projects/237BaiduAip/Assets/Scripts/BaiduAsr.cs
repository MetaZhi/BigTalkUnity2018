using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BaiduAsr : MonoBehaviour
{
    public string APIKey;
    public string SecretKey;
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
            Debug.Log("[BaiduAip]" + www.downloadHandler.text);
            var result = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
            Token = result.access_token;
            Debug.Log("[WitBaiduAip]Token has been fetched successfully");
        }
        else
        {
            Debug.LogError("[BaiduAip]" + www.error);
            Debug.LogError("[BaiduAip]Token was fetched failed. Please check your APIKey and SecretKey");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _clipRecord = Microphone.Start(null, false, 30, 16000);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Microphone.End(null);
            Debug.Log("[WitBaiduAip demo]end record");
            var data = ConvertAudioClipToPCM16(_clipRecord);
            StartCoroutine(Recognize(data, s =>
            {
                var text = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";

                Debug.Log(text);
            }));
        }
    }

    public IEnumerator Recognize(byte[] data, Action<AsrResponse> callback)
    {
        var uri = $"https://vop.baidu.com/server_api?lan=zh&cuid={SystemInfo.deviceUniqueIdentifier}&token={Token}";

        var form = new WWWForm();
        form.AddBinaryData("audio", data);
        var www = UnityWebRequest.Post(uri, form);
        www.SetRequestHeader("Content-Type", "audio/pcm;rate=16000");
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("[WitBaiduAip]" + www.downloadHandler.text);
            callback(JsonUtility.FromJson<AsrResponse>(www.downloadHandler.text));
        }
        else
            Debug.LogError(www.error);
    }

    /// <summary>
    /// 将Unity的AudioClip数据转化为PCM格式16bit数据
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public static byte[] ConvertAudioClipToPCM16(AudioClip clip)
    {
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        var samples_int16 = new short[samples.Length];

        for (var index = 0; index < samples.Length; index++)
        {
            var f = samples[index];
            samples_int16[index] = (short)(f * short.MaxValue);
        }

        var byteArray = new byte[samples_int16.Length * 2];
        Buffer.BlockCopy(samples_int16, 0, byteArray, 0, byteArray.Length);

        return byteArray;
    }
}
