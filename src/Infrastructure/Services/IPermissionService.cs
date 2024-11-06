namespace Infrastructure.Services;

public interface IPermissionService
{
    HashSet<string> GetPermissions();
}
