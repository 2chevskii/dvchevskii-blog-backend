namespace Dvchevskii.Blog.Contracts.Common;

public class AuditInfoDto
{
    public int CreatedById { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public int? DeletedById { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
