namespace BuildingBlocks.Authorization.Models;

public class PermissionAssignmentModel
{
    public string SubjectType { get; set; } // "User", "Role", "Workgroup", "RoleWorkgroup"
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? WorkgroupId { get; set; }
}