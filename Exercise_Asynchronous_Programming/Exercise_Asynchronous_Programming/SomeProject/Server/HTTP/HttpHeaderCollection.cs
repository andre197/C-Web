namespace SomeProject.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            this.headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
            if (this.headers.ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        public HttpHeader GetHeader(string key)
        {
            if (this.headers.ContainsKey(key))
            {
                HttpHeader header = this.headers[key];

                return header;
            }

            throw new ArgumentException("the given key was not found");
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }
    }
}
