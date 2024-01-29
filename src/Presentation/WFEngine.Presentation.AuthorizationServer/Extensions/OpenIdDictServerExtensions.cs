using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using WFEngine.Application.AuthorizationServer.Options;
using WFEngine.Application.Common.Extensions;
using WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.ValueObjects;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
                        configure
                            .SetAuthorizationEndpointUris("authorize")
                            .SetTokenEndpointUris("token")
                            .SetUserinfoEndpointUris("userinfo");

                        configure
                            .AllowAuthorizationCodeFlow()
                            .AllowClientCredentialsFlow()
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

            @this.Services.AddAuthorization();

            return @this;
        }

        public static async Task<WebApplication> UseOpenIdDict(this WebApplication @this, IConfiguration configuration)
        {
            @this.UseAuthentication();
            @this.UseAuthorization();

            var options = configuration.GetOptions<OpenIdDictServerOptions>();

            using var scope = @this.Services.CreateAsyncScope();
            var clientManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            foreach (var client in options.AuthorizationClients)
            {
                if (await clientManager.FindByClientIdAsync(client.ClientId) is null)
                {
                    var clientEntity = new OpenIddictApplicationDescriptor
                    {
                        ClientId = client.ClientId,
                        ClientType = ClientTypes.Public,
                        Requirements = { Requirements.Features.ProofKeyForCodeExchange },
                        Permissions =
                        {
                            Permissions.Endpoints.Authorization,
                            Permissions.Endpoints.Token,
                            Permissions.GrantTypes.AuthorizationCode,
                            Permissions.ResponseTypes.Code,
                            Permissions.Scopes.Email,
                            Permissions.Scopes.Profile,
                            Permissions.Scopes.Roles,
                        }
                    };

                    foreach (var redirectUri in client.RedirectUris)
                        clientEntity.RedirectUris.Add(new Uri(redirectUri));

                    foreach (var redirectUri in client.PostLogoutRedirectUris)
                        clientEntity.PostLogoutRedirectUris.Add(new Uri(redirectUri));

                    await clientManager.CreateAsync(clientEntity);
                }
            }

            return @this;
        }

        internal static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ExecuteAsync();

            async Task<List<T>> ExecuteAsync()
            {
                var list = new List<T>();

                await foreach (var element in source)
                {
                    list.Add(element);
                }

                return list;
            }
        }
    }
}
