using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess;

public class DataBaseContextFactory
{
    // Action encapsulate a method that has a single parameter but has no return value
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    
    public DataBaseContextFactory( Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }

    public DataBaseContext CreateDbContext()
    {
        DbContextOptionsBuilder<DataBaseContext> optionsBuilder = new();
        _configureDbContext(optionsBuilder);
        return new DataBaseContext(optionsBuilder.Options);
    }
}