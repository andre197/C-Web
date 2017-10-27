namespace SomeProject.Server.Handlers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Contracts;
    using HTTP.Contracts;
    using Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            var requestMethod = httpContext.Request.RequestMethod;
            var requestPath = httpContext.Request.Path;
            var allRoutes = this.serverRouteConfig.Routes[requestMethod];

            foreach (var kvp in allRoutes)
            {
                string pattern = kvp.Key;

                Regex regex = new Regex(pattern);
                Match match = regex.Match(requestPath);

                if (!match.Success)
                {
                    continue;
                }

                var parameters = kvp.Value.Parameters;

                foreach (var valueParameter in parameters)
                {
                    httpContext.Request.AddUrlParameters(valueParameter, match.Groups[valueParameter].Value);
                }

                return kvp.Value.RequestHandler.Handle(httpContext);
            }

            throw new InvalidOperationException("Regex or path not valid");
        }
    }
}
