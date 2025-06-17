using Microsoft.AspNetCore.Mvc;
using BuildingBlocks.Authorization.Services;
using BuildingBlocks.Authorization.Models;

namespace AuthorizationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet("{applicationCode}")]
    public async Task<ActionResult<List<PermissionModel>>> GetPermissions(string applicationCode)
    {
        var permissions = await _permissionService.GetPermissionsForApplicationAsync(applicationCode);
        return Ok(permissions);
    }

    [HttpGet("userinfo/{userId:guid}")]
    public async Task<ActionResult<UserInfoModel>> GetUserInfo(Guid userId)
    {
        var userInfo = await _permissionService.GetUserInfoAsync(userId);
        if (userInfo == null)
            return NotFound();
        return Ok(userInfo);
    }

    // Kullanıcının rollerini döner
    [HttpGet("roles/{userId:guid}")]
    public async Task<ActionResult<List<string>>> GetUserRoles(Guid userId)
    {
        var roles = await _permissionService.GetUserRoleNamesAsync(userId);
        if (roles == null)
            return NotFound();
        return Ok(roles);
    }

    // Kullanıcının workgroup'larını döner
    [HttpGet("workgroups/{userId:guid}")]
    public async Task<ActionResult<List<string>>> GetUserWorkgroups(Guid userId)
    {
        var workgroups = await _permissionService.GetUserWorkgroupsAsync(userId);
        if (workgroups == null)
            return NotFound();
        return Ok(workgroups);
    }
}