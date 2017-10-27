namespace SomeProject.Server.Handlers
{
    using System;
    using Contracts;
    using HTTP.Contracts;
    using HTTP.Responses;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> func;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> func)
        {
            this.func = func;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            IHttpResponse response = this.func(httpContext.Request);

            return response;
        }
    }
}
