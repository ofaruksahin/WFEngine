using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WFEngine.Application.AuthorizationServer.Options;
using WFEngine.Application.Common.Extensions;
using WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.ValueObjects;

namespace WFEngine.Presentation.AuthorizationServer.Extensions
{
    internal static class OpenIdDictServerExtensions
    {
        public static WebApplicationBuilder AddOpenIdDictServer(this WebApplicationBuilder @this, IConfiguration configuration)
        {
            var openIdDictServerOptions = configuration.GetOptions<OpenIdDictServerOptions>();
            var connectionStringsOptions = configuration.GetOptions<ConnectionStringOptions>();

            @this.Services.AddDbContext<AuthorizationConfigurationDbContext>(options =>
            {
                options.UseMySQL(connectionStringsOptions.AuthorizationConfigurationDbContext);
                options.UseOpenIddict();
            });

            @this
                .Services
                    .AddOpenIddict()
                    .AddCore(configure =>
                    {
                        configure
                            .UseEntityFrameworkCore()
                            .UseDbContext<AuthorizationConfigurationDbContext>();
                    })
                    .AddServer(configure =>
                    {
                        configure.SetAuthorizationEndpointUris("authorize");

                        configure
                            .AllowAuthorizationCodeFlow()
                        .AllowRefreshTokenFlow();

                        configure.AddEncryptionKey(new SymmetricSecurityKey(
                            Convert.FromBase64String(openIdDictServerOptions.EncryptionKey)));

                        configure.AddDevelopmentSigningCertificate();

                        configure
                            .UseAspNetCore()
                            .EnableAuthorizationEndpointPassthrough();
                    })
                    .AddValidation(options =>
                    {
                        options.UseLocalServer();
                        options.UseAspNetCore();
                    });

            return @this;
        }
    }
}
