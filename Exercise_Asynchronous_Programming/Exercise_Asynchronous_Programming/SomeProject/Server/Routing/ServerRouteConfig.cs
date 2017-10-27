namespace SomeProject.Server.Routing
{
    using Contracts;
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Handlers;

    public class ServerRouteConfig : IServerRouteConfig
    {
        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.Routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();

            var methods = Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod requestMethod in methods)
            {
                this.Routes[requestMethod] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes { get; }

        private void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registerRoutes in appRouteConfig.Routes)
            {
                var requestMethod = registerRoutes.Key;
                var routesWithHandlers = registerRoutes.Value;

                foreach (var requestHandler in routesWithHandlers)
                {
                    string route = requestHandler.Key;
                    RequestHandler handler = requestHandler.Value;

                    List<string> parameters = new List<string>();

                    string parsedRegex = this.ParseRoute(route, parameters);

                    IRoutingContext routingContext = new RoutingContext(parameters, handler);

                    this.Routes[requestMethod].Add(parsedRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string requestHandlerKey, List<string> args)
        {
            if (requestHandlerKey == "/")
            {
                return "^/$";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("^/");

            string[] tokens = requestHandlerKey.Split(new [] {"/"}, StringSplitOptions.RemoveEmptyEntries );

            this.ParseTokens(args, tokens, sb);

            return sb.ToString();
        }

        private void ParseTokens(List<string> args, string[] tokens, StringBuilder sb)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string end = i == tokens.Length - 1 ? "$" : "/";

                if (!tokens[i].StartsWith("{") && !tokens[i].EndsWith("}"))
                {
                    sb.Append($"{tokens[i]}{end}");
                    continue;
                }

                string pattern = "<\\w+>";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(tokens[i]);

                if (!match.Success)
                {
                    throw new InvalidOperationException("Regex does not match the given parameter.");
                }

                string paramName = match.Groups[0].Value.Substring(1, match.Groups[0].Length - 2);

                args.Add(paramName);

                sb.Append($"{tokens[i].Substring(1, tokens[i].Length - 2)}{end}");
            }
        }
    }
}
