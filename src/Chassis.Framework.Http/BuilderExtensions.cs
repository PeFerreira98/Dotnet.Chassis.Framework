using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PeFerreira98.Chassis.Framework.Http.Oidc;
using PeFerreira98.Chassis.Framework.Http.OidcHandler;
using Refit;

namespace PeFerreira98.Chassis.Framework.Http;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder ConfigureClients(this IHostApplicationBuilder builder)
    {
        const string ClientsConfigurationKey = "Clients";

        builder.Services.Configure<ClientConfigurations>(builder.Configuration.GetSection(ClientsConfigurationKey));

        return builder;
    }

    public static IServiceCollection AddHttpClient<T>(this IServiceCollection services)
        where T : class, IRefitClient
    {
        const string clientName = nameof(T);

        services.AddRefitClient<T>()
            .ConfigureHttpClient((sProvider, hClient) => hClient.BaseAddress = GetClientUri(sProvider, clientName));

        return services;
    }

    public static IServiceCollection AddOAuthHttpClient<T>(this IServiceCollection services)
        where T : class, IRefitClient
    {
        string clientName = typeof(T).Name;

        services.AddRefitClient<T>()
            .ConfigureHttpClient((sProvider, hClient) => hClient.BaseAddress = GetClientUri(sProvider, clientName))
            .AddHttpMessageHandler(sProvider => OAuth2HandlerBuilder(sProvider, clientName));

        return services;
    }

    private static Uri GetClientUri(IServiceProvider sProvider, string clientName)
    {
        var configs = sProvider.GetService<ClientConfigurations>();
        var uriConfig = configs.uriCatalog[clientName];
        return uriConfig.Uri;
    }

    private static DelegatingHandler OAuth2HandlerBuilder(IServiceProvider serviceProvider, string clientName)
    {
        var configs = serviceProvider.GetService<IOptions<ClientConfigurations>>().Value;
        var oidcKey = configs.uriCatalog[clientName].Oidc;
        var oidcSetting = configs.oidcCatalog[oidcKey];

        var httpClient = serviceProvider.GetService<IHttpClientFactory>().CreateClient();
        var logger = serviceProvider.GetService<ILogger<IdentityServerClient>>();
        var identityClient = new IdentityServerClient(httpClient, oidcSetting.ClientConfiguration, logger);

        return new OAuth2Handler(identityClient);
    }
}