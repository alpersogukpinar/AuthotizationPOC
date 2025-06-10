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
        var permissions = await _permissionService.GetPermissionsAsync(applicationCode);
        return Ok(permissions);
    }
}