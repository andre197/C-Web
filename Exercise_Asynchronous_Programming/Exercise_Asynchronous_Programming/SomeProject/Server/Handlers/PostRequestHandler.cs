namespace SomeProject.Server.Handlers
{
    using System;
    using HTTP.Contracts;

    public class PostRequestHandler : RequestHandler
    {
        public PostRequestHandler(Func<IHttpRequest, IHttpResponse> func) 
            : base(func)
        { }
    }
}
