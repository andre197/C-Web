namespace SomeProject.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Enums;
    using Handlers;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>>();

            var methods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod requestMethod in methods)
            {
                this.routes.Add(requestMethod, new Dictionary<string, RequestHandler>());
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes => this.routes;

        public void AddRoute(string route, RequestHandler handler)
        {
            var handlerType = handler.GetType().ToString().ToLower();

            if (handlerType.Contains("get"))
            {
                this.routes[HttpRequestMethod.GET][route] = handler;
            }
            else if (handlerType.Contains("post"))
            {
                this.routes[HttpRequestMethod.POST][route] = handler;
            }
        }
    }
}
