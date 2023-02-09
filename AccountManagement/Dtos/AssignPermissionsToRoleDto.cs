namespace AccountManagement.Dtos;

public class AssignPermissionsToRoleDto
{
    public string RoleId { get; set; }
    public List<string> PermissionNames { get; set; }
}
