using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class EchoClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        socket.Connect(new IPEndPoint(ip, 9999));

        socket.Send(Encoding.UTF8.GetBytes("你好呀"));

        var buffer = new byte[1024];
        int length = socket.Receive(buffer);
        Debug.Log(Encoding.UTF8.GetString(buffer));
    }
}
