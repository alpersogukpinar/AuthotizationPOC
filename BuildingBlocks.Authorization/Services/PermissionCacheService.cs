using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Services
{
    public class PermissionCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _authzServiceUrl;

        public PermissionCacheService(IMemoryCache cache, IHttpClientFactory httpClientFactory, string authzServiceUrl)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _authzServiceUrl = authzServiceUrl;
        }

        public async Task CachePermissionsAsync(string applicationCode)
        {
            var client = _httpClientFactory.CreateClient();
            var permissions = await client.GetFromJsonAsync<List<PermissionModel>>(
                $"{_authzServiceUrl}/api/permissions/{applicationCode}");

            if (permissions != null)
                _cache.Set($"permissions:{applicationCode}", permissions, TimeSpan.FromMinutes(30));
        }

        public List<PermissionModel> GetPermissions(string applicationCode)
        {
            return _cache.TryGetValue($"permissions:{applicationCode}", out List<PermissionModel> permissions)
                ? permissions
                : new List<PermissionModel>();
        }
    }
}