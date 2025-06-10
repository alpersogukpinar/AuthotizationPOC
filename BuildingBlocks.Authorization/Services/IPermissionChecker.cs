using BuildingBlocks.Authorization.Models;
using System.Security.Claims;

namespace BuildingBlocks.Authorization.Services;

public interface IPermissionChecker
{
    Task<bool> HasPermissionAsync(string applicationCode, string permissionCode, ClaimsPrincipal user);
}