using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Services
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly List<PermissionModel> _mockPermissions;

        public AuthorizationProvider()
        {
            // Mock data -- gerçekte cache'ten/dış kaynaktan alınacak
            _mockPermissions = new List<PermissionModel>
            {
                new PermissionModel
                {
                    PermissionId = Guid.NewGuid(),
                    Code = "MoneyTransfer.Create",
                    ResourceName = "MoneyTransfer",
                    ActionName = "Create",
                    Subjects = new List<SubjectAssignment>
                    {
                        new SubjectAssignment { SubjectType = "Role", RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") }
                    }
                }
            };
        }

        public Task<List<PermissionModel>> GetPermissionsAsync(string applicationCode)
        {
            // Gerçekte applicationCode'a göre filtreleme yapılacak
            return Task.FromResult(_mockPermissions);
        }

        public Task<bool> HasPermissionAsync(Guid userId, string permissionCode, IEnumerable<Guid> roleIds, IEnumerable<Guid> workgroupIds)
        {
            var permission = _mockPermissions.FirstOrDefault(p => p.Code == permissionCode);
            if (permission == null)
                return Task.FromResult(false);

            bool authorized = permission.Subjects.Any(sub =>
                (sub.SubjectType == "User" && sub.UserId == userId) ||
                (sub.SubjectType == "Role" && sub.RoleId.HasValue && roleIds.Contains(sub.RoleId.Value)) ||
                (sub.SubjectType == "Workgroup" && sub.WorkgroupId.HasValue && workgroupIds.Contains(sub.WorkgroupId.Value)) ||
                (sub.SubjectType == "Role+Workgroup" && sub.RoleId.HasValue && sub.WorkgroupId.HasValue &&
                    roleIds.Contains(sub.RoleId.Value) && workgroupIds.Contains(sub.WorkgroupId.Value))
            );

            return Task.FromResult(authorized);
        }
    }
}