using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoFramework.Web.Startup))]
namespace DoFramework.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
