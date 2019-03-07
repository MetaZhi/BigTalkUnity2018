using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UnityPageRequest : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var url = "https://www.baidu.com";
        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log("status code:" + www.responseCode);
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var text = www.downloadHandler.text;
            var bytes = www.downloadHandler.data;
        }
    }

    IEnumerator RequestTexture()
    {
        var url = "https://www.baidu.com/img/bd_logo1.png";
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Debug.Log("status code:" + www.responseCode);
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var tex = DownloadHandlerTexture.GetContent(www);
        }
    }

    IEnumerator RequestMedia()
    {
        var url = "https://www.baidu.com/img/bd_logo1.png";
        var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
        yield return www.SendWebRequest();

        Debug.Log("status code:" + www.responseCode);
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var audio = DownloadHandlerAudioClip.GetContent(www);
        }
    }

    IEnumerator UploadTexture()
    {
        var tex = new Texture2D(1,1);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();
        // tex.Apply() 对性能影响较大，故等待一帧再执行
        yield return null;
        var bytes = tex.EncodeToPNG();
        var form = new WWWForm();
        form.AddBinaryData("screenshot", bytes);
        var www = UnityWebRequest.Post("server url", form);
        yield return www.SendWebRequest();

        // ...
    }
}
