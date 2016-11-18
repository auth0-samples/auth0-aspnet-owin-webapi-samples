using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using IdentityServer3.AccessTokenValidation;
using Microsoft.IdentityModel.Protocols;
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

            var keyResolver = new OpenIdKeyResolver(domain);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = apiIdentifier,
                        ValidIssuer = domain,
                        IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) => keyResolver.GetSigningKey(identifier)
                    }
                });

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

            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = domain,
            //    IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) =>
            //    {
            //        // Set the valid audience
            //        parameters.ValidAudience = apiIdentifier;

            //        // Get the kid we are looking for
            //        string kid = identifier.OfType<NamedKeySecurityKeyIdentifierClause>().FirstOrDefault()?.Id;

            //        // Get the token with the kid, and return fhe first security key from it
            //        return parameters.IssuerSigningTokens.FirstOrDefault(t => t.Id == kid)?.SecurityKeys?.FirstOrDefault();
            //    }
            //});

            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }

    public class OpenIdKeyResolver
    {
        internal static class AsyncHelper
        {
            private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

            public static void RunSync(Func<Task> func)
            {
                TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
            }

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            {
                return TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
            }
        }

        private OpenIdConnectConfiguration openIdConfig;

        public OpenIdKeyResolver(string authority)
        {
            var cm = new ConfigurationManager<OpenIdConnectConfiguration>($"{authority.TrimEnd('/')}/.well-known/openid-configuration");
            openIdConfig = AsyncHelper.RunSync(async () => await cm.GetConfigurationAsync());

        }

        public SecurityKey GetSigningKey(SecurityKeyIdentifier identifier)
        {
            // Find the security token which matches the identifier
            var securityToken = openIdConfig.SigningTokens.FirstOrDefault(t =>
            {
                // Each identifier has multiple clauses. Try and match for each
                foreach (var securityKeyIdentifierClause in identifier)
                {
                    if (t.MatchesKeyIdentifierClause(securityKeyIdentifierClause))
                        return true;
                }

                return false;
            });

            // Return the first key of the security token (if found)
            return securityToken?.SecurityKeys.FirstOrDefault();
        }
    }

    
}
