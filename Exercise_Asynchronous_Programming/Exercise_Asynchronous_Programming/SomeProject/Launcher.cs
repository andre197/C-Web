namespace SomeProject
{
    using Application;
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    public class Launcher : IRunnable
    {
        private WebServer server;

        public static void Main(string[] args)
        {
            new Launcher().Run();
        }

        public void Run()
        {
            IApplication app = new MainApplication();
            IAppRouteConfig config = new AppRouteConfig();
            
            app.Start(config);

            this.server = new WebServer(8230, config);
            this.server.Run();
        }
    }
}
