using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeanCloudReg : MonoBehaviour
{
    public string AppId;
    public string AppKey;

    public InputField Username;
    public InputField Password;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Reg(){
        var username = Username.text;
        var password = Username.text;

        var url = "https://5jmvfx9e.api.lncld.net/1.1/users";

        UnityWebRequest.Post(url)
    }


}
