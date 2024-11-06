namespace Application.DTOs.Responses
{
    public record RoleResponseWithPermissions(int Id, string Role, string[] Permissions);
}
