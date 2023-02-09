namespace AccountManagement.Dtos;

public class UserDetailDto : UserDto
{
    public List<string> RoleNames { get; set; }
    public List<string> PermissionNames { get; set; }
}
