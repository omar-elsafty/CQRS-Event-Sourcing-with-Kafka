using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PostEntity> Post { get; set; }
    public DbSet<CommentEntity> Comment { get; set; }
}