using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpfInventaire.Web.Startup))]
namespace SpfInventaire.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
