using IdentityModel.Client;

namespace PeFerreira98.Chassis.Framework.Http.Oidc;

public class ClientConfigurations
{
    public UriCatalog uriCatalog { get; set; } = [];
    public OidcCatalog oidcCatalog { get; set; } = [];
}

public class UriCatalog : Dictionary<string, UriSettings>;

public class UriSettings
{
    public Uri Uri { get; set; }
    public string Oidc { get; set; }
    public int? Timeout { get; set; }
}

public class OidcCatalog : Dictionary<string, OidcSettings>;

public class OidcSettings
{
    public OidcMode Mode { get; set; }
    public ClientCredentialsTokenRequest ClientConfiguration { get; set; }
}

public enum OidcMode
{
    ClientCredentials
}