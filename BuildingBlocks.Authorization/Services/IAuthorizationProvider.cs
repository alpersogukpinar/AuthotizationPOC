using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Authorization.Models;

namespace BuildingBlocks.Authorization.Services
{
    public interface IAuthorizationProvider
    {
        Task<bool> HasPermissionAsync(
            Guid userId,
            string permissionCode,
            IEnumerable<Guid> roleIds,
            IEnumerable<Guid> workgroupIds);

        Task<List<PermissionModel>> GetPermissionsAsync(string applicationCode);
    }
}