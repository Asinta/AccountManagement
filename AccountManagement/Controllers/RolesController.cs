using System.Security.Claims;

namespace AccountManagement.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Adapt<List<RoleDto>>();
    }

    [HttpPost]
    public async Task<IActionResult> AddRoleAsync([FromBody] RoleCreationDto roleCreationDto)
    {
        var role = roleCreationDto.Adapt<IdentityRole>();
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return StatusCode(201);
        }
        
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new RoleCreationResponseDto {Errors = errors});
    }

    [HttpPost("assign-permissions")]
    public async Task<IActionResult> AssignPermissionsToRoleAsync([FromBody] AssignPermissionsToRoleDto assignPermissionsToRoleDto)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(u => u.Id == assignPermissionsToRoleDto.RoleId);
        if (role is null)
        {
            return BadRequest("No such role");
        }

        var existingPermissions = await _roleManager.GetClaimsAsync(role);
        var claims = assignPermissionsToRoleDto.PermissionNames.Select(rn => new Claim("Permissions", rn)).ToList();

        foreach (var existingPermission in existingPermissions) await _roleManager.RemoveClaimAsync(role, existingPermission);

        foreach (var claim in claims) await _roleManager.AddClaimAsync(role, claim);

        return Ok();
    }
}
