using Mapster;

namespace AccountManagement.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AccountsController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto? userRegistrationDto)
    {
        if (userRegistrationDto is null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = userRegistrationDto.Adapt<User>();
        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password!);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new UserRegistrationResponseDto {Errors = errors});
        }

        return StatusCode(201);
    }
}