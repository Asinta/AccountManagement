namespace AccountManagement.Dtos;

public class RoleCreationResponseDto
{
    public bool IsSuccessfulCreation { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
