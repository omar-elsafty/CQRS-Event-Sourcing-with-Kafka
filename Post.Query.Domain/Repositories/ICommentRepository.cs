using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ICommentRepository
{
    Task CreateAsync(CommentEntity comment);
    Task UpdateAsync(CommentEntity post);
    Task DeleteAsync(Guid commentId);
    Task<CommentEntity> getByIdAsync(Guid commentId);
}