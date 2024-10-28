namespace Infrastructure.Services;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(string username);
}
