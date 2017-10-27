namespace SomeProject.Server.HTTP
{
    using Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(string request)
        {
            this.request = new HttpRequest(request);
        }

        public IHttpRequest Request => this.request;
    }
}
