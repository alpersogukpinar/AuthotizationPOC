using BuildingBlocks.Authorization.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Authorization.Infrastructure;

public class PermissionRepository : IPermissionRepository
{
    private readonly AuthorizationDbContext _context;

    public PermissionRepository(AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionModel>> GetPermissionsForApplicationAsync(string applicationCode)
    {
        var permissions = await (
            from p in _context.Permissions
            join r in _context.Resources on p.ResourceId equals r.Id
            join a in _context.Actions on p.ActionId equals a.Id
            join app in _context.Applications on r.ApplicationId equals app.Id
            where app.Code == applicationCode
                  && !p.IsDeleted && p.IsActive
                  && !r.IsDeleted && r.IsActive
                  && !a.IsDeleted && a.IsActive
                  && !app.IsDeleted && app.IsActive
            select new PermissionModel
            {
                PermissionId = p.Id,
                Code = p.Code,
                ResourceName = r.Name,
                ActionName = a.Name,
                PermissionAssignments = (
                    from pa in _context.PermissionAssignments // Permission'a atanmış tüm assignment'lar
                    where pa.PermissionId == p.Id
                          && pa.IsActive
                          && !pa.IsDeleted
                    select new PermissionAssignmentModel
                    {
                        AssignmentType = pa.AssignmentType,
                        UserId = pa.UserId,
                        RoleId = pa.RoleId,
                        WorkgroupId = pa.WorkgroupId
                    }
                ).ToList()
            }
        ).ToListAsync();

        return permissions;
    }

    public async Task<UserInfoModel?> GetUserInfoAsync(Guid userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId && !u.IsDeleted && u.IsActive)
            .FirstOrDefaultAsync();
        if (user == null)
            return null;

        var roles = await GetUserRoleNamesAsync(userId);
        var workgroups = await GetUserWorkgroupsAsync(userId); // Parent zincirini de içeren güncel metot
        var workgroupNames = workgroups.Select(wg => wg.Name ?? "").ToList(); // List<string>

        return new UserInfoModel
        {
            Username = user.Username,
            Roles = roles,
            Workgroups = workgroupNames
        };
    }

    public async Task<List<string>> GetUserRoleNamesAsync(Guid userId)
    {
        return await (
            from ur in _context.UserRoles
            join r in _context.Roles on ur.RoleId equals r.Id
            where ur.UserId == userId
                  && !ur.IsDeleted
                  && !r.IsDeleted && r.IsActive
            select r.Name ?? ""
        ).ToListAsync();
    }

    public async Task<List<Workgroup>> GetUserWorkgroupsAsync(Guid userId)
    {
       return await GetUserWorkgroupsWithRawSQLAsync(userId);
    //    return await GetUserWorkgroupsWithLinqAsync(userId);
    }

    private async Task<List<Workgroup>> GetUserWorkgroupsWithRawSQLAsync(Guid userId)
    {
        var sql = @"
            WITH RecursiveGroups AS (
                SELECT Id, Name, ParentId, Description, IsActive, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, ModifiedBy, RowVersion
                FROM Workgroups
                WHERE Id IN (SELECT WorkgroupId FROM UserWorkgroups WHERE UserId = {0})
                UNION ALL
                SELECT wg.Id, wg.Name, wg.ParentId, wg.Description, wg.IsActive, wg.IsDeleted, wg.CreatedDate, wg.CreatedBy, wg.UpdatedDate, wg.ModifiedBy, wg.RowVersion
                FROM Workgroups wg
                INNER JOIN RecursiveGroups rg ON wg.Id = rg.ParentId
                WHERE wg.IsActive = 1 AND wg.IsDeleted = 0
            )
            SELECT DISTINCT Id, Name, ParentId, Description, IsActive, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, ModifiedBy, RowVersion FROM RecursiveGroups
        ";

        return await _context.Workgroups
            .FromSqlRaw(sql, userId)
            .ToListAsync();        
    }

    private async Task<List<Workgroup>> GetUserWorkgroupsWithLinqAsync(Guid userId)
    {
        // Kullanıcının doğrudan workgroup'larını ve parentId'lerini al
        var userGroups = await (
            from uwg in _context.UserWorkgroups
            join wg in _context.Workgroups on uwg.WorkgroupId equals wg.Id
            where uwg.UserId == userId
                  && !uwg.IsDeleted
                  && !wg.IsDeleted && wg.IsActive
            select wg
        ).ToListAsync();

        var allGroups = new Dictionary<Guid, Workgroup>();
        foreach (var wg in userGroups)
            allGroups[wg.Id] = wg;

        // Parent zincirini yukarıya doğru bul
        var parentIds = userGroups
            .Where(w => w.ParentId != null)
            .Select(w => w.ParentId.Value)
            .Distinct()
            .ToList();

        while (parentIds.Any())
        {
            var parents = await _context.Workgroups
                .Where(wg => parentIds.Contains(wg.Id) && !wg.IsDeleted && wg.IsActive)
                .ToListAsync();

            foreach (var parent in parents)
                allGroups[parent.Id] = parent;

            parentIds = parents
                .Where(p => p.ParentId != null && !allGroups.ContainsKey(p.ParentId.Value))
                .Select(p => p.ParentId.Value)
                .Distinct()
                .ToList();
        }

        return allGroups.Values.ToList();
    }


}