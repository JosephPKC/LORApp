using LORCardLoader.Models;

namespace LORCardLoader.Loader.InsertQueries;

internal static class QueryBuilderFactory
{
    public static IQueryBuilder<TModel> CreateQueryBuilder<TModel>() where TModel : BaseModel
    {
        return CreateSqlQueryBuilder<TModel>();
    }

    private static IQueryBuilder<TModel> CreateSqlQueryBuilder<TModel>() where TModel : BaseModel
    {
        return nameof(TModel) switch
        {
            nameof(KeywordModel) => (IQueryBuilder<TModel>)new KeywordSqlQueryBuilder(),
            _ => throw new NotSupportedException($"Type {nameof(TModel)} not supported.")
        };
    }
}