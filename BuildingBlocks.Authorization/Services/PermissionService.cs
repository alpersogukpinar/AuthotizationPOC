using BuildingBlocks.Authorization.Models;
using BuildingBlocks.Authorization.Infrastructure;

namespace BuildingBlocks.Authorization.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _repository;

    public PermissionService(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode)
    {
        return await _repository.GetPermissionsForApplicationAsync(applicationCode);
    }

    public async Task<UserInfoModel?> GetUserInfoAsync(Guid userId)
    {
        return await _repository.GetUserInfoAsync(userId);
    }

    public async Task<List<string>> GetUserRoleNamesAsync(Guid userId)
    {
        return await _repository.GetUserRoleNamesAsync(userId);
    }

    public async Task<List<Workgroup>> GetUserWorkgroupsAsync(Guid userId)
    {
        return await _repository.GetUserWorkgroupsAsync(userId);
    }
}