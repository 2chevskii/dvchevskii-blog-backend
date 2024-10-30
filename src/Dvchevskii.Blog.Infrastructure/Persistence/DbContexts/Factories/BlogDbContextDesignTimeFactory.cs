using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dvchevskii.Blog.Infrastructure.Persistence.DbContexts.Factories;

public class BlogDbContextDesignTimeFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    public BlogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>()
            .UseMySql(
                new MySqlServerVersion("9.1"),
                mysql => { mysql.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name); }
            );

        return new BlogDbContext(optionsBuilder.Options);
    }
}
