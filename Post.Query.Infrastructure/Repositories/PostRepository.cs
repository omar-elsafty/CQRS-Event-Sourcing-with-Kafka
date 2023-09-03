using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DataBaseContextFactory _contextFactory;

    public PostRepository(DataBaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        await context.Post.AddAsync(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Post.Update(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        using var context = _contextFactory.CreateDbContext();
        var post = await context.Post.FirstOrDefaultAsync(e => e.PostId == postId);
        if (post == null) return;
        context.Post.Remove(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task<PostEntity> getByIdAsync(Guid postId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post
            .Include(e => e.Comments)
            .FirstOrDefaultAsync(e => e.PostId == postId);
    }

    public async Task<List<PostEntity>> ListAllAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(e => e.Comments).AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAutherAsync(string auther)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(e => e.Comments).AsNoTracking()
            .Where(e => e.Auther == auther)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithLikesNumber(int numOfLikes)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(e => e.Comments).AsNoTracking()
            .Where(e => e.Likes >= numOfLikes)
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithComments()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(e => e.Comments).AsNoTracking()
            .Where(e => e.Comments != null && e.Comments.Any())
            .ToListAsync();
    }
}