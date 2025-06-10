
namespace BuildingBlocks.Authorization.Models;

public class SubjectAssignment
{
    public string SubjectType { get; set; } // "User", "Role", "Workgroup", "Role+Workgroup"
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? WorkgroupId { get; set; }
}