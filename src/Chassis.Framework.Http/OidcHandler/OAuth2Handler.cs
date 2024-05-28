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

internal interface IIdentityServerClient
{
    Task<string> RequestClientCredentialsTokenAsync();
}

internal class IdentityServerClient(
    HttpClient httpClient,
    ClientCredentialsTokenRequest tokenRequest,
    ILogger<IdentityServerClient> logger) : IIdentityServerClient
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ClientCredentialsTokenRequest _tokenRequest = tokenRequest ?? throw new ArgumentNullException(nameof(tokenRequest));
    private readonly ILogger<IdentityServerClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<string> RequestClientCredentialsTokenAsync()
    {
        var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);

        if (!tokenResponse.IsError)
        {
            return tokenResponse.AccessToken;
        }

        _logger.LogError(tokenResponse.Error);
        throw new HttpRequestException("Something went wrong while requesting the access token");

    }
}