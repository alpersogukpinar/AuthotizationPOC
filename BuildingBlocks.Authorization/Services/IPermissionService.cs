using Microsoft.Extensions.Caching.Memory;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Services;

public interface IPermissionService
{
    Task<List<PermissionModel>> GetPermissionsAsync(string applicationCode);
    Task RefreshPermissionsAsync(string applicationCode);
}