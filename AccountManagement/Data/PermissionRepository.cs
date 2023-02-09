namespace AccountManagement.Data;

public class PermissionRepository : IPermissionRepository
{
    private readonly AccountDbContext _context;

    public PermissionRepository(AccountDbContext context)
    {
        _context = context;
    }

    public async Task AddPermissionsAsync(IEnumerable<string> permissionNames)
    {
        var existingPermission = await _context.Permissions.Select(p => p.Name).ToListAsync();
        var dupPermissions = permissionNames.Where(pn => !existingPermission.Contains(pn)).Select(p => new Permission
        {
            Id = Guid.NewGuid(),
            Name = p
        });

        await _context.Permissions.AddRangeAsync(dupPermissions);
        await _context.SaveChangesAsync();
    }
}
