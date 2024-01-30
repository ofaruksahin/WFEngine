using WFEngine.Domain.Common.Contracts;

namespace WFEngine.Application.AuthorizationServer.Options
{
    public class OpenIdDictServerOptions : IOptions
    {
        public string Key => "WFEngine:AuthorizationServer:OpenIdDictServer";

        public string EncryptionKey { get; set; }
        public int AccessTokenLifetimeInMinute { get; set; }
        public int RefreshTokenLifetimeInMinute { get; set; }
        public OpenIdDictAuthorizationClient[] AuthorizationClients { get; set; }

        public OpenIdDictServerOptions()
        {
            AuthorizationClients = new OpenIdDictAuthorizationClient[0];
        }

        public class OpenIdDictAuthorizationClient
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string[] RedirectUris { get; set; }
            public string[] PostLogoutRedirectUris { get; set; }
            public bool IsWebClient { get; set; }

            public OpenIdDictAuthorizationClient()
            {
                RedirectUris = new string[0];
                PostLogoutRedirectUris = new string[0];
            }
        }
    }
}
