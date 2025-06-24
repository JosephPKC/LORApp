using System.Data;

using LORApp.Entities;

namespace LORApp.Services.CardRepo.Mappers;

public interface IModelMapperManager
{
    TModel Map<TModel>(DataTable pDt) where TModel : BaseModel;
    IEnumerable<TModel> MapList<TModel>(DataTable pDt) where TModel : BaseModel;
}
