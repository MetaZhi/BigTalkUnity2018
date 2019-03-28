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

            // 4. accept客户端的连接，如果客户端不连接，服务端程序会阻塞（挂起）在这个位置
            var clientSocket = socket.Accept();

            // 5. 接收客户端消息，如果客户端不发送数据，服务端会阻塞（挂起）在这个位置
            var buffer = new byte[1024];
            int length = clientSocket.Receive(buffer);

            Console.WriteLine($"接收到客户端的消息:{Encoding.UTF8.GetString(buffer)}");

            // 6. 将收到的消息返回给客户端
            clientSocket.Send(buffer);

            // 关闭两个socket
            clientSocket.Close();
            socket.Close();

            // 接收一个键盘输入的字符，目的是不让命令行自动关闭
            Console.ReadKey();
        }
    }
}
