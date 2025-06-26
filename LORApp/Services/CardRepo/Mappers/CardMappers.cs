using System.Data;
using System.Diagnostics;

using LORApp.Entities.Cards;
using LORApp.Entities.Refs;
using LORApp.Services.CardRepo.Caching;

namespace LORApp.Services.CardRepo.Mappers;

/// <summary>
/// Handles mapping of datatable/datarow for all card, card listings.
/// </summary>
internal static class CardMappers
{
    public static TCard? MapCard<TCard>(DataTable pDt) where TCard : CardModel
    {
        if (pDt.Rows.Count == 0)
        {
            return null;
        }

        DataRow dr = pDt.Rows[0];
        return typeof(TCard).Name switch
        {
            nameof(AbilityCardModel) => MapAbilityCard(dr) as TCard,
            nameof(ChampionCardModel) => MapChampionCard(dr) as TCard,
            nameof(EquipmentCardModel) => MapEquipmentCard(dr) as TCard,
            nameof(LandmarkCardModel) => MapLandmarkCard(dr) as TCard,
            nameof(SpellCardModel) => MapSpellCard(dr) as TCard,
            nameof(TrapCardModel) => MapTrapCard(dr) as TCard,
            nameof(UnitCardModel) => MapUnitCard(dr) as TCard,
            _ => null
        };
    }

    public static AbilityCardModel MapAbilityCard(DataRow pDr)
    {
        AbilityCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static ChampionCardModel MapChampionCard(DataRow pDr)
    {
        ChampionCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static EquipmentCardModel MapEquipmentCard(DataRow pDr)
    {
        EquipmentCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static LandmarkCardModel MapLandmarkCard(DataRow pDr)
    {
        LandmarkCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static SpellCardModel MapSpellCard(DataRow pDr)
    {
        SpellCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static TrapCardModel MapTrapCard(DataRow pDr)
    {
        TrapCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    public static UnitCardModel MapUnitCard(DataRow pDr)
    {
        UnitCardModel card = new();
        AppendCardShell(card, pDr);
        return card;
    }

    private static void AppendCardShell(CardModel pModel, DataRow pDr)
    {
        pModel.CardCode = MapperUtils.GetString(pDr, "CardCode");
        pModel.Name = MapperUtils.GetString(pDr, "Name");
        pModel.Type = MapperUtils.GetEnum<CardTypes>(pDr, "CardType");
        pModel.Rarity = MapperUtils.GetEnum<CardRarities>(pDr, "CardRarity");
        pModel.Cost = MapperUtils.GetInt(pDr, "Cost");
        pModel.Artist = MapperUtils.GetString(pDr, "ArtistName");
        pModel.ArtUriPath = MapperUtils.GetString(pDr, "ArtImagePath");
        pModel.Description = MapperUtils.GetString(pDr, "Description");
        pModel.DescriptionFormatted = MapperUtils.GetString(pDr, "DescriptionFormatted");
        pModel.FlavorText = MapperUtils.GetString(pDr, "FlavorText");
        pModel.IsCollectible = MapperUtils.GetBool(pDr, "IsCollectible");
    }

    public static void AppendChampionCardInfo(ChampionCardModel pModel, DataTable pChampionDt)
    {
        if (pChampionDt.Rows.Count == 0)
        {
            return;
        }

        DataRow dr = pChampionDt.Rows[0];
        pModel.Attack = MapperUtils.GetInt(dr, "Attack");
        pModel.Health = MapperUtils.GetInt(dr, "Health");
        pModel.LevelUpDescription = MapperUtils.GetString(dr, "LevelUpDescription");
        pModel.LevelUpDescriptionFormatted = MapperUtils.GetString(dr, "LevelUpDescriptionFormatted");
    }

    public static void AppendUnitCardInfo(UnitCardModel pModel, DataTable pUnitDt)
    {
        if (pUnitDt.Rows.Count == 0)
        {
            return;
        }

        DataRow dr = pUnitDt.Rows[0];
        pModel.Attack = MapperUtils.GetInt(dr, "Attack");
        pModel.Health = MapperUtils.GetInt(dr, "Health");
    }

    public static void AppendAssociatedCards(CardModel pModel, DataTable pAssocCardDt)
    {
        List<string> assocCards = [];
        foreach (DataRow dr in pAssocCardDt.Rows)
        {
            assocCards.Add(MapperUtils.GetString(dr, "AssociatedCardCode"));
        }
        pModel.AssociatedCardCodes = assocCards;
    }

    public static void AppendCardKeywords(CardModel pModel, DataTable pKeywordDt, RefCache pCache)
    {
        List<KeywordModel> keywords = [];
        foreach (DataRow dr in pKeywordDt.Rows)
        {
            string refCode = MapperUtils.GetString(dr, "RefCode");
            if (!pCache.Keywords.TryGetValue(refCode, out KeywordModel? keyword))
            {
                Trace.WriteLine($"Keyword refcode: {refCode} not found in cache.");
                continue;
            }

            if (keyword is null)
            {
                continue;
            }

            keywords.Add(keyword);
        }
        pModel.Keywords = keywords;
    }

    public static void AppendCardRegions(CardModel pModel, DataTable pRegionDt, RefCache pCache)
    {
        List<RegionModel> regions = [];
        foreach (DataRow dr in pRegionDt.Rows)
        {
            string refCode = MapperUtils.GetString(dr, "RefCode");
            if (!pCache.Regions.TryGetValue(refCode, out RegionModel? region))
            {
                Trace.WriteLine($"Region refcode: {refCode} not found in cache.");
                continue;
            }

            if (region is null)
            {
                continue;
            }

            regions.Add(region);
        }
        pModel.Regions = regions;
    }

    public static void AppendCardSubtypes(CardModel pModel, DataTable pSubtypeDt)
    {
        List<string> subtypes = [];
        foreach (DataRow dr in pSubtypeDt.Rows)
        {
            subtypes.Add(MapperUtils.GetString(dr, "Subtype"));
        }
        pModel.SubTypes = subtypes;
    }
}