namespace Domain.Enums;

[Flags]
public enum Permissions
{
    None = 0,
    CanRead = 1,
    CanAdd = 2,
    CanUpdate = 4,
    CanDelete = 8,

    All = ~None
}