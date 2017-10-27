namespace SomeProject.Application.Controller
{
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Responses;
    using Views;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpResponseStatusCode.OK, new HomeIndexView());
        }
    }
}
