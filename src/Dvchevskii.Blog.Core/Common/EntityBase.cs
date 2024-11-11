namespace Dvchevskii.Blog.Core.Common;

public abstract class EntityBase
{
    public int Id { get; set; }
    public AuditInfo AuditInfo { get; set; } = new();

    /*public void EnsureAuditInfoCreated()
    {
        if (AuditInfo == null!)
        {
            AuditInfo = new AuditInfo();
        }
    }*/
}
