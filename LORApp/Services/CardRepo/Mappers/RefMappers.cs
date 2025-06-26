using System.Data;

using LORApp.Entities.Refs;

namespace LORApp.Services.CardRepo.Mappers;

internal static class RefMappers
{
    public static IEnumerable<KeywordModel> MapKeywordList(DataTable pDt)
    {
        return MapperUtils.GetAllModels(pDt, MapKeyword);
    }

    public static KeywordModel MapKeyword(DataRow pDr)
    {
        return new()
        {
            RefCode = MapperUtils.GetString(pDr, "RefCode"),
            Name = MapperUtils.GetString(pDr, "Name"),
            Description = MapperUtils.GetString(pDr, "Description")
        };
    }

    public static IEnumerable<RegionModel> MapRegionList(DataTable pDt)
    {
        return MapperUtils.GetAllModels(pDt, MapRegion);
    }

    public static RegionModel MapRegion(DataRow pDr)
    {
        return new()
        {
            RefCode = MapperUtils.GetString(pDr, "RefCode"),
            Name = MapperUtils.GetString(pDr, "Name"),
            IconPath = MapperUtils.GetString(pDr, "IconPath")
        };
    }
}