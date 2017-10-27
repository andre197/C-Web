namespace SomeProject.Server.Contracts
{
    using Routing.Contracts;

    public interface IApplication
    {
        void Start(IAppRouteConfig appRouteConfig);
    }
}
