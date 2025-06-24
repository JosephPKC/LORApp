using LORApp.Entities;

namespace LORApp.Services.CardRepo.Caching;

internal interface IRepoCache
{
    TModel? Get<TModel>(string pKey) where TModel : BaseModel;
    bool Put<TModel>(string pKey, TModel pModel) where TModel : BaseModel;
}