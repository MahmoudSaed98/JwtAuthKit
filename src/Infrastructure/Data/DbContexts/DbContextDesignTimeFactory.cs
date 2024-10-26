using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data.DbContexts;

internal class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer("Server=.;Database=JwtDemo;Integrated Security=SSPI;Trust Server Certificate=true;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
