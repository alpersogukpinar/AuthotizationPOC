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

    public async Task<List<PermissionModel>> GetPermissionsAsync(string applicationCode)
    {
        // Doğrudan veritabanından çek
        return await _repository.GetPermissionsForApplicationAsync(applicationCode);
    }

    public async Task RefreshPermissionsAsync(string applicationCode)
    {
        // Cache kullanılmadığı için burada bir işlem yapılmasına gerek yok
        await Task.CompletedTask;
    }
}