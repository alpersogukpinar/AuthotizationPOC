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

    [HttpGet("userinfo/{username}")]
    public async Task<ActionResult<UserInfoModel>> GetUserInfo(string username)
    {
        var userInfo = await _permissionService.GetUserInfoAsync(username);
        if (userInfo == null)
            return NotFound();
        return Ok(userInfo);
    }
}