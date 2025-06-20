using LORCardLoader.Models;

namespace LORCardLoader.CardLoader;

internal interface IDbGateway
{
    void Create(string pCreateQueryString);
    void Insert<TModel>(TModel pModel) where TModel : BaseModel;
    void InsertAll<TModel>(IEnumerable<TModel> pModels) where TModel : BaseModel;
}
