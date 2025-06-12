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

    public async Task<UserInfoModel?> GetUserInfoAsync(string username)
    {
        var user = await _context.Users
            .Where(u => u.Username == username && !u.IsDeleted && u.IsActive) // Kullanıcıyı bul
            .FirstOrDefaultAsync();
        if (user == null)
            return null;

        var roles = await (
            from ur in _context.UserRoles // Kullanıcının rollerini bul
            join r in _context.Roles on ur.RoleId equals r.Id
            where ur.UserId == user.Id
                  && !ur.IsDeleted
                  && !r.IsDeleted && r.IsActive
            select r.Name
        ).ToListAsync();

        var workgroups = await (
            from uwg in _context.UserWorkgroups // Kullanıcının workgroup'larını bul
            join wg in _context.Workgroups on uwg.WorkgroupId equals wg.Id
            where uwg.UserId == user.Id
                  && !uwg.IsDeleted
                  && !wg.IsDeleted && wg.IsActive
            select wg.Name
        ).ToListAsync();

        return new UserInfoModel
        {
            Username = user.Username,
            Roles = roles,
            Workgroups = workgroups
        };
    }
}