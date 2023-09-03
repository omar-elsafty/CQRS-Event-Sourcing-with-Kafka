using CQRS.Core.Queries;

namespace Post.Query.Api.Queries;

public class FindPostByAutherQuery : BaseQuery
{
    public string Auther { get; set; }
}