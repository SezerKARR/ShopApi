using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shop.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
        "server=localhost;port=3306;database=shopapi;user=root;password=1234",
        new MySqlServerVersion(new Version(8, 0, 34))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}

