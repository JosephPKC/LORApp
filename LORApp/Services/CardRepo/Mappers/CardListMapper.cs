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
        CardListingModel card = new()
        {
            CardCode = pDr["CardCode"].ToString() ?? string.Empty,
            Name = pDr["Name"].ToString() ?? string.Empty,
            Cost = (int)pDr["Cost"],
            CardType = (CardTypes)pDr["CardType"],
            CardRarity = (CardRarities)pDr["CardRarity"]
        };

        List<string> SearchableText = [];
        AppendSearchableText(SearchableText, pDr, "Name");
        AppendSearchableText(SearchableText, pDr, "Description");
        card.SearchableText = SearchableText;

        return card;
    }

    public static void AppendCardListingRegion(CardListingModel pModel, DataTable pRegionsDt, RefCache pCache)
    {
        //  Adds region icons to the listing model, as it is not native to the card data table.
        if (pRegionsDt.Rows.Count > 0)
        {
            pModel.RegionIcon1 = GetCardRegionIconPath(pRegionsDt.Rows[0], pCache) ?? string.Empty;
            pModel.Region1 = pRegionsDt.Rows[0]["Name"].ToString() ?? string.Empty;
        }

        if (pRegionsDt.Rows.Count > 1)
        {
            pModel.RegionIcon1 = GetCardRegionIconPath(pRegionsDt.Rows[1], pCache) ?? string.Empty;
            pModel.Region1 = pRegionsDt.Rows[1]["Name"].ToString() ?? string.Empty;
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

    public static void AppendKeywordsAsSearchableText(
        CardListingModel pCard,
        DataTable pKeywordsDt,
        RefCache pCache)
    {
        List<string> searchableTexts = [.. pCard.SearchableText];

        foreach (DataRow keywordDr in pKeywordsDt.Rows)
        {
            string? refCode = keywordDr["RefCode"].ToString();
            if (string.IsNullOrWhiteSpace(refCode))
            {
                Trace.WriteLine($"Keyword refcode for {pCard.CardCode} is missing.");
                continue;
            }

            if (!pCache.Keywords.TryGetValue(refCode, out KeywordModel? keyword) || keyword is null)
            {
                Trace.WriteLine($"Keyword {refCode} not found in cache.");
                continue;
            }

            searchableTexts.Add(keyword.Name);
        }

        pCard.SearchableText = searchableTexts;
    }

    public static void AppendChampionSearchableText(CardListingModel pCard, DataRow pChampionDr)
    {
        List<string> searchableTexts = [.. pCard.SearchableText];
        AppendSearchableText(searchableTexts, pChampionDr, "LevelUpDescription");
        pCard.SearchableText = searchableTexts;
    }

    private static void AppendSearchableText(ICollection<string> pTexts, DataRow pDr, string pField)
    {
        string? text = pDr[pField].ToString();
        if (!string.IsNullOrWhiteSpace(text))
        {
            pTexts.Add(text);
        }
    }
}
