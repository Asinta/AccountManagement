using System.Security.Claims;

namespace AccountManagement.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUserAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return BadRequest("No such user!");
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok();
        }

        return StatusCode(500, "Delete failed");
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Adapt<List<UserDto>>();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailDto>> GetUserDetailAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return BadRequest("No such user");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userPermissions = await _userManager.GetClaimsAsync(user);
        var rolePermissions = new List<Claim>();
        foreach (var role in roles)
        {
            var roleEntity = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);
            var permissions = await _roleManager.GetClaimsAsync(roleEntity!);
            rolePermissions.AddRange(permissions);
        }

        var totalPermissions = userPermissions.Union(rolePermissions);

        var userDetail = user.Adapt<UserDetailDto>();
        userDetail.RoleNames = roles.ToList();
        userDetail.PermissionNames = totalPermissions.Select(tp => tp.Value).ToList();

        return Ok(userDetail);
    }

    [HttpPost("assign-roles")]
    public async Task<IActionResult> AssignRolesToUserAsync([FromBody] AssignRoleToUserDto assignRoleToUserDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == assignRoleToUserDto.UserId);
        if (user is null)
        {
            return BadRequest("No such user");
        }

        var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        if (!assignRoleToUserDto.RoleNames.All(r => roles.Contains(r)))
        {
            return BadRequest("some of the roles not exist");
        }

        var existingRoles = await _userManager.GetRolesAsync(user);
        
        await _userManager.RemoveFromRolesAsync(user, existingRoles);
        await _userManager.AddToRolesAsync(user, assignRoleToUserDto.RoleNames);

        return Ok();
    }
}
