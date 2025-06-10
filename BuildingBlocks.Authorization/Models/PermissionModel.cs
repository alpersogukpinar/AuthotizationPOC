namespace BuildingBlocks.Authorization.Models;

public class PermissionModel
{
    public Guid PermissionId { get; set; }
    public string Code { get; set; }
    public string ResourceName { get; set; }
    public string ActionName { get; set; }
    public List<SubjectAssignment> Subjects { get; set; } = new();
}