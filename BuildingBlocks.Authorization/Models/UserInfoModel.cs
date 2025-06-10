namespace BuildingBlocks.Authorization.Models;

public class UserInfoModel
{
    public string Username { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Workgroups { get; set; } = new();
}