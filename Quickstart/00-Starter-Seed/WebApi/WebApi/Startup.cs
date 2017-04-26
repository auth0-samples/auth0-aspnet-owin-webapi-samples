using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Linq;
using Microsoft.Owin.Security.OAuth;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
