namespace SomeProject.Server.HTTP.Responses
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
            : base(redirectUrl)
        { }
    }
}
