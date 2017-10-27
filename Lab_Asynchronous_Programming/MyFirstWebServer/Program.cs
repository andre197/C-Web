using System;

namespace MyFirstWebServer
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            int port = 1300;
            var ipAddress = IPAddress.Parse("127.0.0.1");

            TcpListener listener = new TcpListener(ipAddress, port);

            Console.WriteLine("Server Started");
            Console.WriteLine($"Listning to: {ipAddress}:{port}");

            Task.Run(async () =>
                {
                    await ConnectWithTCPClientAsync(listener);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static async Task ConnectWithTCPClientAsync(TcpListener listener)
        {
            while (true)
            {
                listener.Start();

                Console.WriteLine("Waiting for client ...");

                var client = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected");

                byte[] buffer = new byte[1024];

                client.GetStream().Read(buffer, 0, buffer.Length);

                var message = Encoding.ASCII.GetString(buffer);

                Console.WriteLine(message);

                string newLine = Environment.NewLine;
                string helloMessage = "Hello from my server!";
                string version = "HTTP/1.1";
                string statusCode = "200 OK";
                string contentType = "Content-Type: text/plain";


                byte[] data = Encoding.ASCII.GetBytes($"{version}{statusCode}{newLine}{contentType}{newLine}{newLine}{helloMessage}");

                client.GetStream().Write(data, 0, data.Length);

                Console.WriteLine("Closing connection.");
                client.Dispose();
            }

        }
    }
}
