using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.Authorization.Policies
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        public string PermissionCode { get; }
        public PermissionsRequirement(string permissionCode)
        {
            PermissionCode = permissionCode;
        }
    }
}