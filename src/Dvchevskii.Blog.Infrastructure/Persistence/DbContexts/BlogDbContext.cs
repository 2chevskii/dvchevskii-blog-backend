using Dvchevskii.Blog.Core.Authentication.Users;
using Dvchevskii.Blog.Core.Common;
using Dvchevskii.Blog.Core.Content.Files;
using Dvchevskii.Blog.Core.Content.Posts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;

public class BlogDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Image> Images { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .Where(x => x.ClrType.IsAssignableTo(typeof(EntityBase)))
            .Select(x => x.ClrType)
            .ToList()
            .ForEach(x =>
            {
                modelBuilder.Entity(x)
                    .OwnsOne(typeof(AuditInfo), nameof(EntityBase.AuditInfo))
                    .HasOne(nameof(AuditInfo.CreatedBy))
                    .WithMany();

                modelBuilder.Entity(x)
                    .OwnsOne(typeof(AuditInfo), nameof(EntityBase.AuditInfo))
                    .HasOne(nameof(AuditInfo.UpdatedBy))
                    .WithMany();

                modelBuilder.Entity(x)
                    .OwnsOne(typeof(AuditInfo), nameof(EntityBase.AuditInfo))
                    .HasOne(nameof(AuditInfo.DeletedBy))
                    .WithMany();
            });
    }
}
