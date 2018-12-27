using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASITHmsMvc.Startup))]
namespace ASITHmsMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
