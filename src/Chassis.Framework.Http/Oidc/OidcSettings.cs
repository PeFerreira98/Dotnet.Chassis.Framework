namespace PeFerreira98.Chassis.Framework.Http.Oidc;

public class OidcSettings
{
    public OidcMode Mode { get; set; }
    public required ClientCredentialsTokenRequest ClientConfiguration { get; set; }
}
