using LORCardLoader.Models;

namespace LORCardLoader.Loader.InsertQueries;

internal interface IQueryBuilder<TModel> where TModel : BaseModel
{
    string BuildInsertQuery(TModel pModel);
}