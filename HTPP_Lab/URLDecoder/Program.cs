namespace URLDecoder
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class Program
    {
        public static void Main()
        {
            //DecodeUrl();
            //ValidateURL();
            //ParseRequest();
        }

        private static void ParseRequest()
        {
            Dictionary<string, HashSet<string>> dic = new Dictionary<string, HashSet<string>>();

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "END")
                {
                    break;
                }

                var tokens = input.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                var path = tokens[0];
                var method = tokens[1];

                if (!dic.ContainsKey(path))
                {
                    dic[path] = new HashSet<string>();
                }

                dic[path].Add(method);
            }

            var request = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var requestMethod = request[0];
            var requestPath = request[1].Substring(1);
            var requestProtocol = request[2];

            var responseStatusCode = 404;
            var responseStatusText = "NotFound";

            if (dic.ContainsKey(requestPath) && dic[requestPath].Contains(requestMethod.ToLower()))
            {
                responseStatusCode = 200;
                responseStatusText = "OK";
            }

            Console.WriteLine($"{requestProtocol} {responseStatusCode} {responseStatusText}");
            Console.WriteLine($"Content-Length: {responseStatusText.Length}");
            Console.WriteLine($"Content-Type: text/plain");
            Console.WriteLine();
            Console.WriteLine(responseStatusText);
        }

        private static void ValidateURL()
        {
            var url = Console.ReadLine();

            var urlTokens = new Uri(url);

            var protocol = urlTokens.Scheme;
            var host = urlTokens.Host;
            var port = urlTokens.Port;
            var path = urlTokens.AbsolutePath;
            var query = urlTokens.Query;
            var fragment = urlTokens.Fragment;

            if (protocol == null || host == null || path == null)
            {
                Console.WriteLine("Invalid Url");
                return;
            }

            Console.WriteLine($"Protocol: {protocol}");
            Console.WriteLine($"Host: {host}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Path: {path}");

            if (!string.IsNullOrEmpty(query))
            {
                Console.WriteLine($"Query: {query.Substring(1)}");
            }

            if (!string.IsNullOrEmpty(fragment))
            {
                Console.WriteLine($"Fragment: {fragment.Substring(1)}");
            }
        }

        private static void DecodeUrl()
        {
            var url = Console.ReadLine();

            string decodedUrl = WebUtility.UrlDecode(url);

            Console.WriteLine(decodedUrl);
        }
    }
}


