namespace SomeProject.Application.Controller
{
    using Server;
    using Server.Enums;
    using Server.HTTP.Contracts;
    using Server.HTTP.Responses;
    using Views;

    public class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpResponseStatusCode.OK, new RegisterView());
        }

        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"/user/{name}");
        }

        public IHttpResponse Details(string name)
        {
            Model model = new Model{["name"] = name};

            return new ViewResponse(HttpResponseStatusCode.OK, new UserDetailsView(model));
        }
    }
}
