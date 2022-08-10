namespace CompileTech.Common.Permissions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class RequiredPermissionAttribute : Attribute
{
    public string Resource { get; }
    public string Operation { get; }

    public RequiredPermissionAttribute(string resource, string operation)
    {
        Operation = operation;
        Resource = resource;
    }

    public string ToPermissionToken()
    {
        return $"{Resource}-{Operation}";
    }
}