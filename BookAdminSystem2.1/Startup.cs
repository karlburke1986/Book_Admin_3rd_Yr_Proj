using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StConlethsBookSystem_v2._1.Startup))]
namespace StConlethsBookSystem_v2._1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
