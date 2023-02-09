namespace AccountManagement.Data;

public interface IPermissionRepository
{
    Task AddPermissionsAsync(IEnumerable<string> permissionNames);
}
