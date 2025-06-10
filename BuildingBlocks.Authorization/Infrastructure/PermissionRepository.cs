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
        // Application, Resource, Action, Permission, SubjectPermission join
        var permissions = await (
            from p in _context.Permissions
            join r in _context.Resources on p.ResourceId equals r.Id
            join a in _context.Actions on p.ActionId equals a.Id
            join app in _context.Applications on r.ApplicationId equals app.Id
            where app.Code == applicationCode
            select new PermissionModel
            {
                PermissionId = p.Id,
                Code = p.Code,
                ResourceName = r.Name,
                ActionName = a.Name,
                Subjects = (
                    from sp in _context.SubjectPermissions
                    where sp.PermissionId == p.Id
                    select new SubjectAssignment
                    {
                        SubjectType = sp.SubjectType,
                        UserId = sp.UserId,
                        RoleId = sp.RoleId,
                        WorkgroupId = sp.WorkgroupId
                    }
                ).ToList()
            }
        ).ToListAsync();

        return permissions;
    }

    public async Task<UserInfoModel?> GetUserInfoAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return null;

        var roles = await (
            from ur in _context.UserRoles
            join r in _context.Roles on ur.RoleId equals r.Id
            where ur.UserId == user.Id
            select r.Name
        ).ToListAsync();

        var workgroups = await (
            from uwg in _context.UserWorkgroups
            join wg in _context.Workgroups on uwg.WorkgroupId equals wg.Id
            where uwg.UserId == user.Id
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