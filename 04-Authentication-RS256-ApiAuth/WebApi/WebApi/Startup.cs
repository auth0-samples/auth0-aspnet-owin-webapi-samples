using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using IdentityServer3.AccessTokenValidation;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var domain = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var apiIdentifier = ConfigurationManager.AppSettings["Auth0ApiIdentifier"];

            //string certificatePath = HostingEnvironment.MapPath("~/certificate.cer");
            //var certificate = new X509Certificate2(certificatePath);
            //app.UseJwtBearerAuthentication(
            //    new JwtBearerAuthenticationOptions
            //    {
            //        AuthenticationMode = AuthenticationMode.Active,
            //        TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidAudience = apiIdentifier,
            //            ValidIssuer = domain,
            //            IssuerSigningKey = new X509SecurityKey(certificate),
            //            IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) => parameters.IssuerSigningKey
            //        }
            //    });

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = domain,
                IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) =>
                {
                    // Set the valid audience
                    parameters.ValidAudience = apiIdentifier;

                    // Get the security key
                    var key = parameters.IssuerSigningTokens.FirstOrDefault()?.SecurityKeys?.FirstOrDefault();
                    return key;
                }
            });

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
