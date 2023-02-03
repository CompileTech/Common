using System.Reflection;
using System.Text;

namespace CompileTech.Common.Permissions;

public static class PermissionsHelper
{
    public static IEnumerable<string> GetAllPermissions(Assembly assembly, bool includeRestricted = false)
    {
        return assembly.GetTypes()
            .SelectMany(type => type.GetMembers())
            .Union(assembly.GetTypes())
            .Where(type => Attribute.IsDefined(type, typeof(RequiredPermissionAttribute)))
            .Select(Attribute.GetCustomAttributes)
            .Select(t => (RequiredPermissionAttribute)t.First(a => a is RequiredPermissionAttribute))
            .Where(a=> includeRestricted || !a.Restricted)
            .Select(a => a.ToPermissionToken())
            .OrderBy(a => a)
            .Distinct();
    }

    public static string GeneratePermissionsTsFile(Assembly assembly)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"/* Generated File ({DateTime.Now:MM/dd/yyyy hh:mm:ss tt 'UTC'z}) */");
        sb.AppendLine("export type { Permission };");
        sb.AppendLine($"declare type Permission = {string.Join(" | ", GetAllPermissions(assembly, true).Select(p => $"'{p}'"))};");

        return sb.ToString();
    }

    public static bool CanUser(string pipedUserPermissionTokens, IEnumerable<RequiredPermissionAttribute> requiredPermissions)
    {
        var userPermissionTokens = pipedUserPermissionTokens.Split('|', StringSplitOptions.RemoveEmptyEntries).Distinct();
        var requiredPermissionTokens = requiredPermissions.Select(p => p.ToPermissionToken()).Distinct();
        return requiredPermissionTokens.All(r => userPermissionTokens.Contains(r));
    }
}