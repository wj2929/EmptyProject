using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmptyProject.Manage.Startup))]
namespace EmptyProject.Manage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
