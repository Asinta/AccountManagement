namespace AccountManagement.Controllers;

[ApiController]
[Route("api/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IPermissionRepository _permissionRepository;

    public PermissionsController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IPermissionRepository permissionRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionRepository = permissionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermissionsAsync([FromBody] PermissionCreationDto permissionCreationDto)
    {
        await _permissionRepository.AddPermissionsAsync(permissionCreationDto.PermissionNames);
        return StatusCode(201);
    }
}
