using Microsoft.Extensions.Caching.Memory;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Services;

public interface IPermissionService
{
    Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode);
    Task<UserInfoModel?> GetUserInfoAsync(string username);
    
    
}