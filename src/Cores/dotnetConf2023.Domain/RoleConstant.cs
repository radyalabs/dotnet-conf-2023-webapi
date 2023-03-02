namespace dotnetConf2023.Domain;

public static class RoleConstant
{
    public static readonly Dictionary<string, string> Dictionary = new()
    {
        { Administrator, "Administrator" },
        { User, "User" }
    };

    public const string Administrator = "ADMINISTRATOR";
    public const string User = "USER";
}