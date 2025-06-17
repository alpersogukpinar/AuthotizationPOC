using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Infrastructure;

public interface IPermissionRepository
{
    Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode);
    Task<UserInfoModel?> GetUserInfoAsync(Guid userId);
    Task<List<string>> GetUserRoleNamesAsync(Guid userId);
    Task<List<Workgroup>> GetUserWorkgroupsAsync(Guid userId);
}