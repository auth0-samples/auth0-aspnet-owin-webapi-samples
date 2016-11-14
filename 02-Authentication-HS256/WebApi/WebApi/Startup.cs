using System.Configuration;
using System.Text;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var audience = ConfigurationManager.AppSettings["Auth0ClientID"];
            var secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Auth0ClientSecret"]);

            // If your secret is base-64 encoded the comment the line above, and uncomment this following line
            //var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["Auth0ClientSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    },
                });

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
