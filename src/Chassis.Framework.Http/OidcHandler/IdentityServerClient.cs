namespace PeFerreira98.Chassis.Framework.Http.OidcHandler;

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