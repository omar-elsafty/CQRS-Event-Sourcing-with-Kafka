using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class CommentsRepository :ICommentRepository
{
    private readonly DataBaseContextFactory _contextFactory;

    public CommentsRepository(DataBaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(CommentEntity comment)
    {
        using var context = _contextFactory.CreateDbContext();
        await context.Comment.AddAsync(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Comment.Update(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        using var context = _contextFactory.CreateDbContext();
        var comment = await context.Comment.FirstOrDefaultAsync(e => e.PostId == commentId);
        if (comment == null) return;
        context.Comment.Remove(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> getByIdAsync(Guid commentId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Comment
            .FirstOrDefaultAsync(e => e.CommentId == commentId);
    }
}