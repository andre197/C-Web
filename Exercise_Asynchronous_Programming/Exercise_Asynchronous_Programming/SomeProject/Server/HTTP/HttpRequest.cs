namespace SomeProject.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Enums;
    using Exceptions;
    using System.Web;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, string>();
            this.HeaderCollection = new HttpHeaderCollection();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData { get; protected set; }

        public HttpHeaderCollection HeaderCollection { get; protected set; }

        public string Path { get; protected set; }

        public Dictionary<string, string> QueryParameters { get; protected set; }

        public HttpRequestMethod RequestMethod { get; protected set; }

        public string Url { get; protected set; }

        public Dictionary<string, string> UrlParameters { get; protected set; }

        public void AddUrlParameters(string key, string value)
        {
            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] requestLine = requestLines.First().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line");
            }

            this.RequestMethod = this.ParseRequestMethod(requestLine[0].ToUpper());

            this.Url = requestLine[1];
            this.Path = this.Url.Split(new[] { "#", "?" }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLines.Last(), this.FormData);
            }
        }

        private HttpRequestMethod ParseRequestMethod(string method)
        {
            HttpRequestMethod requestMethod = (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), method);

            return requestMethod;
        }

        private void ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < endIndex; i++)
            {
                string[] headerArgs = requestLines[i].Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                HttpHeader header = new HttpHeader(headerArgs[0], headerArgs[1].Trim());

                this.HeaderCollection.Add(header);
            }

            if (!this.HeaderCollection.ContainsKey("Host"))
            {
                throw new BadRequestException("No host was provided!");
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            string query = this.Url.Split('?').Last();

            this.ParseQuery(query, this.QueryParameters);
        }

        private void ParseQuery(string queries, Dictionary<string, string> dic)
        {
            if (!queries.Contains("="))
            {
                return;
            }

            string[] queryArgs = queries.Split('&');

            foreach (var queryArg in queryArgs)
            {
                string[] query = queryArg.Split('=');

                if (query.Length != 2)
                {
                    continue;
                }

                dic.Add(HttpUtility.UrlDecode(query[0]), HttpUtility.UrlDecode(query[1]));
            }
        }
    }
}
