using Dvchevskii.Blog.Core.Authentication.Users;
using Dvchevskii.Blog.Core.Common;
using Dvchevskii.Blog.Core.Files;
using Dvchevskii.Blog.Core.Posts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Image> Images { get; set; }

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
