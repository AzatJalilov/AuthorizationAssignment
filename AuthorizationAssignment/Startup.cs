using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthorizationAssignment.Startup))]
namespace AuthorizationAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
