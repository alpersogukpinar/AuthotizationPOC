using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Infrastructure;

public interface IPermissionRepository
{
    Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode);
}