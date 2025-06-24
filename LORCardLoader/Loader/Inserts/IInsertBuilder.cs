using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal interface IInsertBuilder<TModel> where TModel : BaseModel
{
    IEnumerable<string> BuildInsertValues(TModel pModel);
}