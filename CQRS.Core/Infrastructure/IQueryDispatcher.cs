using CQRS.Core.Queries;

namespace CQRS.Core.Infrastructure;

public interface IQueryDispatcher<TEntity>
{
    // Two ways of doing this
    // 1 - 
    void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;
    
    // 2 -
    // void RegisterHandler(Func<BaseQuery, Task<List<TEntity>>> handler);
    
    // 1 - 
    Task<List<TEntity>> SendAsync(BaseQuery query);
    
    // 2 -
    // Task<List<TEntity>> SendAsync<TQuery>(TQuery query) where TQuery : BaseQuery;
}