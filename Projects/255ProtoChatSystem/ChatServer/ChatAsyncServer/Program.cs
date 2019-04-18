using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ChatServer().Init();

            // 接收一个键盘输入的字符，目的是不让命令行自动关闭
            Console.ReadKey();
        }
    }

    class Client
    {
        // 定义一个Buffer长度的常量
        public const ushort BUFFER_LENGTH = 1024;
        public Socket Socket;
        public byte[] Buffer = new byte[BUFFER_LENGTH];
    }

    class ChatServer
    {
        Dictionary<Socket, Client> ClientList = new Dictionary<Socket, Client>();

        public void Init()
        {
            // 1. 创建socket对象
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // 2. 绑定IP和端口
            socket.Bind(new IPEndPoint(IPAddress.Any, 9999));

            // 3. 开启监听
            socket.Listen(100);

            // 4. 开始异步accept客户端的连接，不会阻塞
            socket.BeginAccept(AcceptCallback, socket);
            Console.WriteLine($"服务端启动成功");
        }


        private void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            Socket clientSocket = socket.EndAccept(ar);
            Console.WriteLine($"有新的客户端连接：{clientSocket.RemoteEndPoint}");

            // 新建一个客户端对象
            Client client = new Client();
            client.Socket = clientSocket;

            ClientList.Add(clientSocket, client);

            // 5. 异步接收客户端消息，接收到数据后会回调到ReceiveCallback方法
            byte[] buffer = client.Buffer;
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);

            // 继续Accept
            socket.BeginAccept(AcceptCallback, socket);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // 注意此处元组 拆箱 操作的方法
            Client client = ar.AsyncState as Client;
            Socket clientSocket = client.Socket;
            byte[] buffer = client.Buffer;

            try
            {
                int length = client.Socket.EndReceive(ar);
                if (length > 0)
                {
                    Console.WriteLine($"接收到客户端的消息:{Encoding.UTF8.GetString(client.Buffer, 0, length)}");

                    // 6. 将收到的消息返回给所有客户端，优化了发送的字节数量，只发送有数据内容的长度
                    foreach (KeyValuePair<Socket, Client> keyValue in ClientList)
                    {
                        keyValue.Key.Send(buffer, length, SocketFlags.None);
                    }

                    // 重新开始接收
                    clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);
                }
                else // 接收到长度为0的数据，代表客户端断开连接
                {
                    OnClientDisconnect(clientSocket);
                }
            }
            catch (SocketException ex) // 如果服务端有向客户端A未发送完的数据，客户端A主动断开时会触发10054异常，在此捕捉
            {
                if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    OnClientDisconnect(clientSocket);
            }
        }

        private void OnClientDisconnect(Socket clientSocket)
        {
            Console.WriteLine($"客户端断开连接：{clientSocket.RemoteEndPoint}");

            // 移除client列表中该客户端连接
            ClientList.Remove(clientSocket);

            clientSocket.Close();
        }
    }
}
