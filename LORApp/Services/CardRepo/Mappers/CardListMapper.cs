using System.Data;
using System.Diagnostics;

using LORApp.Entities.Cards;
using LORApp.Entities.Listing;
using LORApp.Entities.Refs;
using LORApp.Services.CardRepo.Caching;

namespace LORApp.Services.CardRepo.Mappers;

internal static class CardListMapper
{
    public static CardListModel MapCardList(DataTable pDt)
    {
        return new()
        {
            CardListing = MapperUtils.GetAllModels(pDt, MapCardListing)
        };
    }

    public static CardListingModel MapCardListing(DataRow pDr)
    {
        return new()
        {
            CardCode = pDr["CardCode"].ToString() ?? string.Empty,
            Name = pDr["Name"].ToString() ?? string.Empty,
            Cost = (int)pDr["Cost"],
            CardType = (CardTypes)pDr["CardType"],
            CardRarity = (CardRarities)pDr["CardRarity"],
            RegionIcon1 = pDr["RegionIcon1"].ToString() ?? string.Empty,
            RegionIcon2 = pDr["RegionIcon2"].ToString() ?? string.Empty,
        };
    }

    public static void AppendCardListingRegion(CardListingModel pModel, DataTable pRegionsDt, RefCache pCache)
    {
        //  Adds region icons to the listing model, as it is not native to the card data table.
        if (pRegionsDt.Rows.Count > 0)
        {
            pModel.RegionIcon1 = GetCardRegionIconPath(pRegionsDt.Rows[0], pCache) ?? string.Empty;
        }

        if (pRegionsDt.Rows.Count > 1)
        {
            pModel.RegionIcon1 = GetCardRegionIconPath(pRegionsDt.Rows[1], pCache) ?? string.Empty;
        }
    }

    private static string? GetCardRegionIconPath(DataRow pDr, RefCache pCache)
    {
        string regionRefCode = MapperUtils.GetString(pDr, "RefCode");
        if (!pCache.Regions.TryGetValue(regionRefCode, out RegionModel? region))
        {
            Trace.WriteLine($"Region refcode: {regionRefCode} not found in cache.");
            return null;
        }
        return region.IconPath;
    }
}
