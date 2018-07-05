using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using System.Text;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var audience = ConfigurationManager.AppSettings["Auth0ApiIdentifier"];
            var secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Auth0ApiSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, secret)
                    }
                });

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
