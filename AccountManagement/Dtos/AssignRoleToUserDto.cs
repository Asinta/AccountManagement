namespace AccountManagement.Dtos;

public class AssignRoleToUserDto
{
    public string UserId { get; set; }
    public List<string> RoleNames { get; set; }
}
