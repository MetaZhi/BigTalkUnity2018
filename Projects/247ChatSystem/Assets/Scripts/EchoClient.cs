using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EchoClient : MonoBehaviour
{
    public InputField Input;
    public Text ReceivedText;
    private Socket socket;
    private byte[] buffer = new byte[1024];

    void Start()
    {
        // 1. 创建socket对象
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        // 2. 连接服务端
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        socket.Connect("127.0.0.1", 9999);
    }

    public void Send()
    {
        // 3. 发送数据
        socket.Send(Encoding.UTF8.GetBytes(Input.text));

        // 4. 接收数据
        int length = socket.Receive(buffer);
        var str = Encoding.UTF8.GetString(buffer, 0, length);
        Debug.Log(str);
        ReceivedText.text = ReceivedText.text + str.Trim() + "\n";
    }
}
