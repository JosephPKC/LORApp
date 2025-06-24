using LORApp.Entities.Listing;
using LORApp.Entities.Refs;

namespace LORApp.Services.CardRepo.Mappers;

internal class ModelMapperManager : IModelMapperManager
{
    #region IModelMapperManager
    public TModel Map<TModel>(DataTable pDt) where TModel : BaseModel
    {
        if (pDt.Rows.Count == 0)
        {
            return default;
        }

        DataRow dr = pDt.Rows[0];
        return typeof(TModel).Name switch
        {
            nameof(CardListModel) => CardListMapper.MapCardList(pDt),
            nameof(CardListingModel) => CardListMapper.MapCardListing(dr),
            nameof(KeywordModel) => RefMappers.MapKeyword(dr),
            nameof(RegionModel) => RefMappers.MapRegion(dr),
            _ => default
        };
    }

    public IEnumerable<TModel> MapList<TModel>(DataTable pDt) where TModel : BaseModel
    {
        return typeof(TModel).Name switch
        {
            nameof(KeywordModel) => RefMappers.MapKeywordList(pDt),
            nameof(RegionModel) => RefMappers.MapRegionList(pDt),
            _ => []
        };
    }
    #endregion
}
