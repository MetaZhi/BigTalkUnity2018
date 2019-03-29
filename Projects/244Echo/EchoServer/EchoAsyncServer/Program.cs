using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
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

            // 接收一个键盘输入的字符，目的是不让命令行自动关闭
            Console.ReadKey();
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            var clientSocket = socket.EndAccept(ar);
            Console.WriteLine($"有新的客户端连接：{clientSocket.RemoteEndPoint}");

            // 5. 接收客户端消息，如果客户端不发送数据，服务端会阻塞（挂起）在这个位置
            var buffer = new byte[1024];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, (clientSocket, buffer));

            // 继续Accept
            socket.BeginAccept(AcceptCallback, socket);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            // 注意此处元组 拆箱 操作的方法
            (var clientSocket, var buffer) = ((Socket, byte[]))ar.AsyncState;
            int length = clientSocket.EndReceive(ar);
            if (length > 0)
            {
                Console.WriteLine($"接收到客户端的消息:{Encoding.UTF8.GetString(buffer, 0, length)}");

                // 6. 将收到的消息返回给客户端，优化了发送的字节数量，只发送有数据内容的长度
                clientSocket.Send(buffer, length, SocketFlags.None);

                // 重新开始接收
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, (clientSocket, buffer));
            }
            else
            {
                Console.WriteLine($"客户端断开连接：{clientSocket.RemoteEndPoint}");
                clientSocket.Close();
            }
        }
    }
}
