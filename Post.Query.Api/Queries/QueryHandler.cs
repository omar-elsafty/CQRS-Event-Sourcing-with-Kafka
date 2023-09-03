using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries;

public class QueryHandler : IQueryHandler
{
    private readonly IPostRepository _postRepository;

    public QueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
    {
        return await _postRepository.ListAllAsync();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
    {
        var post = await _postRepository.getByIdAsync(query.PostId);
        return new List<PostEntity> { post };
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostByAutherQuery query)
    {
        return await _postRepository.ListByAutherAsync(query.Auther);
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
    {
        return await _postRepository.ListWithComments();
    }

    public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
    {
        return await _postRepository.ListWithLikesNumber(query.NumberOfLikes);
    }
}