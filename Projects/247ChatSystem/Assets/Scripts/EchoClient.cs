using System;
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

    // 缓存接收到的新消息
    private List<string> Messages = new List<string>();

    void Start()
    {
        // 1. 创建socket对象
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        // 2. 连接服务端
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        socket.Connect("127.0.0.1", 9999);

        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    void Update()
    {
        if (Messages.Count > 0)
        {
            foreach (var str in Messages)
            {
                ReceivedText.text = ReceivedText.text + str + "\n";
            }
            Messages.Clear(); //清除处理过的消息
        }
    }


    // 4. 接收数据
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            if (length > 0)
            {
                var str = Encoding.UTF8.GetString(buffer, 0, length);
                Debug.Log($"接收到服务端的消息:{str}");
                Messages.Add(str);

                // 重新开始接收
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            else // 接收到长度为0的数据，代表断开连接
            {
                OnServerDisconnect();
            }
        }
        catch (SocketException ex) // 如果socket有未发送完的数据，断开时会触发10054异常，在此捕捉
        {
            if (ex.SocketErrorCode == SocketError.ConnectionReset)
                OnServerDisconnect();
        }
    }

    private void OnServerDisconnect()
    {
        Debug.Log("与服务端断开连接");

        socket.Close();
    }

    // 点击按钮后调用Send方法
    public void Send()
    {
        // 3. 发送数据
        socket.Send(Encoding.UTF8.GetBytes(Input.text));
    }
}
