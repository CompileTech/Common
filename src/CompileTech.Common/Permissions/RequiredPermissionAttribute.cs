namespace CompileTech.Common.Permissions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class RequiredPermissionAttribute : Attribute
{
    public string Resource { get; }
    public string Operation { get; }
    public bool Restricted { get; }

    public RequiredPermissionAttribute(string resource, string operation, bool restricted = false)
    {
        Operation = operation;
        Resource = resource;
        Restricted = restricted;
    }

    public string ToPermissionToken()
    {
        return $"{Resource}-{Operation}";
    }
}