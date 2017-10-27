namespace SomeProject.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Contracts;
    using Routing;
    using Routing.Contracts;

    public class WebServer : IRunnable
    {
        private const string ip = "127.0.0.1";

        private readonly int port;

        private readonly IServerRouteConfig serverRouteConfig;

        private readonly TcpListener listener;

        private bool isRunning;

        public WebServer(int port, IAppRouteConfig routeConfig)
        {
            this.port = port;
            this.serverRouteConfig = new ServerRouteConfig(routeConfig);
            this.listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started. Listening to TCP client at {ip}:{this.port}");

            Task task = Task.Run(this.ListenLoop);

            task.Wait();
        }

        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                Socket client = await this.listener.AcceptSocketAsync();

                ConnectionHandler handler = new ConnectionHandler(client, this.serverRouteConfig);

                Task connection = handler.ProcessRequestAsync();

                connection.Wait();
            }
        }
    }
}
