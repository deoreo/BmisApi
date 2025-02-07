namespace BmisApi.Logging
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NoAuditLogAttribute : Attribute
    {
    }
}
