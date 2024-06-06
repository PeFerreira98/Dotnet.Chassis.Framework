namespace PeFerreira98.Chassis.Framework.Http.OidcHandler;

internal class OAuth2Handler(IIdentityServerClient identityServerClient) : DelegatingHandler
{
    private readonly IIdentityServerClient _identityServerClient =
        identityServerClient ?? throw new ArgumentNullException(nameof(identityServerClient));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await _identityServerClient.RequestClientCredentialsTokenAsync();

        request.SetBearerToken(accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}