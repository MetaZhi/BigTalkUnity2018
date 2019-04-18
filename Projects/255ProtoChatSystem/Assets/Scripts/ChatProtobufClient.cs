using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatSystem;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
// Timestamp类型需要
using Google.Protobuf.WellKnownTypes;

public class ChatProtobufClient : MonoBehaviour
{
    public InputField Input;
    public InputField Nickname;
    public Text ReceivedText;
    private Socket socket;
    private byte[] buffer = new byte[1024];

    // 用来存储接收到的数据
    List<byte> DataCache = new List<byte>();

    // 缓存接收到的新消息
    private List<ChatMessage> Messages = new List<ChatMessage>();

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
                // protobuf的Timestamp要求使用Utc时间传输，在这转为本地时间
                ReceivedText.text = ReceivedText.text + str.Nickname + "说：" + str.Content + " " + str.SendTime.ToDateTime().ToLocalTime() +"\n";
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
                DataCache.AddRange(buffer.Take(length));
                ProcessData();

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

    private void ProcessData()
    {
        while (true)
        {
            var data = Decode();
            if (data == null)
                break;

            var chatMsg = ChatMessage.Parser.ParseFrom(data);

            Debug.Log($"接收到服务端的消息:{chatMsg.Content}");
            Messages.Add(chatMsg);
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
        var chatMsg = new ChatMessage()
        {
            Nickname = Nickname.text,
            Content = Input.text,
            // protobuf的Timestamp要求使用Utc时间传输
            SendTime = Timestamp.FromDateTime(DateTime.UtcNow)
        };

        byte[] msg = Encode(chatMsg.ToByteArray());

        // 3. 发送数据
        socket.Send(msg);
    }

    // 封包
    byte[] Encode(byte[] data)
    {

        byte[] jsonBytes = data;

        var length = BitConverter.GetBytes((ushort)jsonBytes.Length);
        byte[] bytes = new byte[jsonBytes.Length + 2];

        Buffer.BlockCopy(length, 0, bytes, 0, 2);
        Buffer.BlockCopy(jsonBytes, 0, bytes, 2, jsonBytes.Length);

        return bytes;
    }

    // 解包
    byte[] Decode()
    {
        if (DataCache.Count < 2) return null;

        var length = BitConverter.ToUInt16(DataCache.Take(2).ToArray(), 0);
        // 判断数据是否足够，如果不够可能原因是发生分包，下次再解析
        if (DataCache.Count - 2 >= length)
        {
            var data = DataCache.Skip(2).Take(length + 2).ToArray();
            DataCache.RemoveRange(0, length + 2);
            return data;
        }

        return null;
    }

    void OnDestroy()
    {
        socket.Close();
    }
}
