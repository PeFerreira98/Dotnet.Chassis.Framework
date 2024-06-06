namespace PeFerreira98.Chassis.Framework.Http.OidcHandler;

internal interface IIdentityServerClient
{
    Task<string> RequestClientCredentialsTokenAsync();
}
