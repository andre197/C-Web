namespace SomeProject.Server.Handlers
{
    using System;
    using HTTP.Contracts;

    public class GetRequestHandler : RequestHandler
    {
        public GetRequestHandler(Func<IHttpRequest, IHttpResponse> func) 
            : base(func)
        { }
    }
}
