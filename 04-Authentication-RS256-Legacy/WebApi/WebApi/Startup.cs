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
            var issuer = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var audience = ConfigurationManager.AppSettings["Auth0ClientID"];

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseActiveDirectoryFederationServicesBearerAuthentication(
                new ActiveDirectoryFederationServicesBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = audience,
                        ValidIssuer = issuer,
                        IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) => parameters.IssuerSigningTokens.FirstOrDefault()?.SecurityKeys?.FirstOrDefault()
                    },
                    // Setting the MetadataEndpoint so the middleware can download the RS256 certificate
                    MetadataEndpoint = $"{issuer.TrimEnd('/')}/wsfed/{audience}/FederationMetadata/2007-06/FederationMetadata.xml"
                });
            

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
