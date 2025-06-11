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
        // Doğrudan veritabanından çek
        return await _repository.GetPermissionsForApplicationAsync(applicationCode);
    }

    public async Task<UserInfoModel?> GetUserInfoAsync(string username)
    {
        return await _repository.GetUserInfoAsync(username);
    }
}