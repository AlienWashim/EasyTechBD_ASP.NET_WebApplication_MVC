using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DynamicProductUpdate.Startup))]
namespace DynamicProductUpdate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
