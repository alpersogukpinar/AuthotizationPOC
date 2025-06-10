using System.Security.Claims;
using BuildingBlocks.Authorization.Services;

namespace BuildingBlocks.Authorization.Services;
public class PermissionChecker : IPermissionChecker
{
    private readonly IPermissionService _cacheService;

    public PermissionChecker(IPermissionService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<bool> HasPermissionAsync(string applicationCode, string permissionCode, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("UserId claim yok!"));
        var roleIds = user.FindAll("role").Select(c => Guid.Parse(c.Value)).ToList();
        var workgroupIds = user.FindAll("workgroup").Select(c => Guid.Parse(c.Value)).ToList();

        var permissions = await _cacheService.GetPermissionsForApplicationAsync(applicationCode);

        // .All kodunu da kontrol et
        var allPermissionCode = permissionCode.Split('.').First() + ".All";

        var permission = permissions.FirstOrDefault(p => p.Code == permissionCode || p.Code == allPermissionCode);
        if (permission == null) return false;

        return permission.Subjects.Any(subject =>
            (subject.SubjectType == "User" && subject.UserId == userId) ||
            (subject.SubjectType == "Role" && subject.RoleId != null && roleIds.Contains(subject.RoleId.Value)) ||
            (subject.SubjectType == "Workgroup" && subject.WorkgroupId != null && workgroupIds.Contains(subject.WorkgroupId.Value)) ||
            (subject.SubjectType == "RoleWorkgroup" && subject.RoleId != null && subject.WorkgroupId != null &&
                roleIds.Contains(subject.RoleId.Value) && workgroupIds.Contains(subject.WorkgroupId.Value))
        );
    }
}