using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Authorization.Models;

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
}