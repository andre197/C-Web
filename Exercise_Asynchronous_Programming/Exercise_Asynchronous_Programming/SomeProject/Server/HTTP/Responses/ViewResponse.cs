namespace SomeProject.Server.HTTP.Responses
{
    using Enums;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        public ViewResponse(HttpResponseStatusCode responseCode, IView view)
            : base(responseCode, view)
        { }
    }
}
