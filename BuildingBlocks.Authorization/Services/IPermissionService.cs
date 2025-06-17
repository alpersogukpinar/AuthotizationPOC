using Microsoft.Extensions.Caching.Memory;
using BuildingBlocks.Authorization.Models;
using BuildingBlocks.Authorization.Infrastructure;

namespace BuildingBlocks.Authorization.Services;

public interface IPermissionService
{
    Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode);
    Task<UserInfoModel?> GetUserInfoAsync(Guid userId);
    Task<List<string>> GetUserRoleNamesAsync(Guid userId);
    Task<List<Workgroup>> GetUserWorkgroupsAsync(Guid userId);
}