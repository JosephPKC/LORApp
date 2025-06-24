using System.Collections.Generic;
using System.Data;

using LORApp.Entities.Refs;

namespace LORApp.Services.CardRepo.Mappers;

internal static class RefMappers
{
    public static IEnumerable<KeywordModel> MapKeywordList(DataTable pDt)
    {
        List<KeywordModel> models = [];
        foreach (DataRow dr in pDt.Rows)
        {
            models.Add(MapKeyword(dr));
        }
        return models;
    }

    public static KeywordModel MapKeyword(DataRow pDr)
    {
        return new()
        {
            RefCode = pDr["RefCode"],
            Name = pDr["Name"],
            Description = pDr["Description"]
        };
    }

    public static IEnumerable<RegionModel> MapRegionList(DataTable pDt)
    {
        List<RegionModel> models = [];
        foreach (DataRow dr in pDt.Rows)
        {
            models.Add(MapRegion(dr));
        }
        return models;
    }

    public static RegionModel MapRegion(DataRow pDr)
    {
        return new()
        {
            RefCode = pDr["RefCode"],
            Name = pDr["Name"],
            IconPath = pDr["IconPath"]
        };
    }
}