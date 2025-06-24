using System.Data;
using LORApp.Entities.Cards;
using LORApp.Entities.Listing;

namespace LORApp.Services.CardRepo.Mappers;

internal static class CardListMapper
{
    public static CardListModel MapCardList(DataTable pDt)
    {
        List<CardListingModel> cardListing = [];
        foreach (DataRow dr in pDt.Rows)
        {
            cardListing.Add(MapCardListing(dr));
        }
        return new()
        {
            CardListing = cardListing
        };
    }

    public static CardListingModel MapCardListing(DataRow pDr)
    {
        string? isFavorite = pDr["IsFavorite"].ToString();
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
}
