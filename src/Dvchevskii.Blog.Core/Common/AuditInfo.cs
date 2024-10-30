using System.ComponentModel;
using Dvchevskii.Blog.Core.Authentication.Users;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Core.Common;

[Owned]
public class AuditInfo
{
    [DefaultValue(typeof(DateTime), "1970-01-01T00:00:00.0000000Z")]
    public DateTime CreatedAtUtc { get; set; }

    [DefaultValue(0)] public int CreatedById { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
    public int? UpdatedById { get; set; }

    public DateTime? DeletedAtUtc { get; set; }
    public int? DeletedById { get; set; }

    public User CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }
    public User? DeletedBy { get; set; }
}
