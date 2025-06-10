using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using BuildingBlocks.Authorization.Models;
using BuildingBlocks.Authorization.Services;
using BuildingBlocks.Authorization.Helpers;
using Microsoft.AspNetCore.Http;


namespace BuildingBlocks.Authorization.Policies
{
    public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
    {
        private readonly PermissionCacheService _permissionCacheService;
        private readonly string _applicationCode;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsHandler(PermissionCacheService permissionCacheService, string applicationCode, IHttpContextAccessor httpContextAccessor)
        {
            _permissionCacheService = permissionCacheService;
            _applicationCode = applicationCode;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            // Permissionları cache'den al
            var permissions = _permissionCacheService.GetPermissions(_applicationCode);

            // JWT'yi Authorization header'dan al
            var httpContext = _httpContextAccessor.HttpContext;
            var jwt = httpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwt))
                return Task.CompletedTask;

            var (userId, username, roles, workgroups) = JwtHelper.ParseClaims(jwt);

            
            var hasPermission = permissions.Any(p =>
                p.Code == requirement.PermissionCode &&
                p.Subjects.Any(s =>
                    (s.SubjectType == "Role" && s.RoleId != null && roles.Contains(s.RoleId.ToString())) ||
                    (s.SubjectType == "User" && s.UserId != null && s.UserId.ToString() == userId) ||
                    (s.SubjectType == "Workgroup" && s.WorkgroupId != null && workgroups.Contains(s.WorkgroupId.ToString())) ||
                    (s.SubjectType == "RoleWorkgroup" && s.RoleId != null && s.WorkgroupId != null &&
                        roles.Contains(s.RoleId.ToString()) && workgroups.Contains(s.WorkgroupId.ToString()))
                )
            );

            if (hasPermission)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}