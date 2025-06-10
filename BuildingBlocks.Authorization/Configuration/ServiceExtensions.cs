using BuildingBlocks.Authorization.Policies;
using BuildingBlocks.Authorization.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Authorization.Configuration;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthorizationModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Permission cache servisini ekle
        services.AddSingleton<PermissionCacheService>(provider =>
            new PermissionCacheService(
                provider.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>(),
                provider.GetRequiredService<System.Net.Http.IHttpClientFactory>(),
                configuration.GetValue<string>("AuthorizationServiceUrl") ?? "https://localhost:7190"
            )
        );

        services.AddSingleton<PermissionsHandler>(provider =>
            new PermissionsHandler(
                provider.GetRequiredService<PermissionCacheService>(),
                configuration.GetValue<string>("ApplicationCode") ?? "MoneyTransferService",
                provider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>()
            )
        );
        services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler>(provider =>
            provider.GetRequiredService<PermissionsHandler>()
        );

        // Dinamik policy provider
        services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, DynamicPermissionPolicyProvider>();

        return services;
    }
}