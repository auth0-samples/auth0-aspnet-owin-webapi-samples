using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var domain = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var apiIdentifier = ConfigurationManager.AppSettings["Auth0ApiIdentifier"];

            string certificatePath = HostingEnvironment.MapPath("~/auth0.cer");
            var certificate = new X509Certificate2(certificatePath);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = apiIdentifier,
                        ValidIssuer = domain,
                        IssuerSigningKeyResolver = (a, b, c, d) => new X509SecurityKey(certificate),
                    }
                });

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }   
}
