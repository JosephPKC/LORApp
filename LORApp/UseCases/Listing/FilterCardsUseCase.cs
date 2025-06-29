using System.Diagnostics;
using System.Text.RegularExpressions;
using LORApp.Entities.Cards;
using LORApp.Entities.Listing;
using LORApp.Utils.Filters;

namespace LORApp.UseCases.Listing;

internal enum CardSortTypes
{
    Name,
    Cost,
    Type,
    Rarity,
    Region
}

internal enum CardSortOrders
{
    Asc,
    Desc
}

internal enum CardSearchTypes
{
    Normal,
    Regex
}

internal enum CardFilterRegions
{
    BandleCity,
    Bilgewater,
    Demacia,
    Freljord,
    Ionia,
    Noxus,
    PiltoverZaun,
    ShadowIsles,
    Shurima,
    Targon
}

internal enum CardFilterTypes
{
    Ability,
    Champion,
    Equipment,
    Landmark,
    Spell,
    Trap,
    Unit
}

internal enum CardFilterRarities
{
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Champion
}

internal enum CardFilterCosts
{
    Cost0,
    Cost1,
    Cost2,
    Cost3,
    Cost4,
    Cost5,
    Cost6,
    Cost7,
    Cost8,
    Cost9,
    Cost10Plus
}

internal enum CardFilterCollectable
{
    Collectable,
    NonCollectable
}

internal enum FilterTypes
{
    Specific,
    All,
    None
}

internal class FilterSet<TKey> where TKey : notnull, Enum
{
    public FilterTypes FilterType { get; set; } = FilterTypes.Specific;
    public HashSet<TKey>? SpecificFilters { get; set; }
}

internal class CardListFilter
{
    //  Sorts the list based on this field.
    //  Always defaults to Name.
    //  Tie breakers are in this order: Name, Cost, Type, Rarity
    public CardSortTypes SortType { get; set; } = CardSortTypes.Name;
    public CardSortOrders SortOrder { get; set; } = CardSortOrders.Asc;
    //  Filters based on a regex supported search in name, description, and keywords
    public string SearchFilterExp { get; set; } = string.Empty;
    public CardSearchTypes SearchType { get; set; } = CardSearchTypes.Normal;

    //  Select filter based on region, type, rarity
    //  Can select all, none, or any combination in between.
    public FilterSet<CardFilterCosts> CostFilters { get; set; } = new();
    public FilterSet<CardFilterRarities> RarityFilters { get; set; } = new();
    public FilterSet<CardFilterRegions> RegionFilters { get; set; } = new();
    public FilterSet<CardFilterTypes> TypeFilters { get; set; } = new();
    public FilterSet<CardFilterCollectable> CollectableFilters { get; set; } = new();
}

internal class FilterCardsUseCase
{
    private List<CardListingModel> _cardList = [];

    public void LoadCardList(IEnumerable<CardListingModel> pCardList)
    {
        _cardList = [.. pCardList];
    }

    public IEnumerable<CardListingModel> FilterCardList(CardListFilter pFilters)
    {
        IQueryable<CardListingModel> cardListQuery = _cardList.AsQueryable();
        //  Filter based on search exp
        if (!string.IsNullOrWhiteSpace(pFilters.SearchFilterExp))
        {
            cardListQuery = FilterOnSearchExp(cardListQuery, pFilters.SearchFilterExp, pFilters.SearchType);
        }

        //  Filter based on filters
        cardListQuery = FilterOnMisc(cardListQuery, pFilters);

        //  Sort the list
        cardListQuery = SortList(cardListQuery, pFilters);

        return [.. cardListQuery];
    }

    private IQueryable<CardListingModel> FilterOnSearchExp(IQueryable<CardListingModel> pCardList, string pSearchExp, CardSearchTypes pSearchType)
    {
        switch (pSearchType)
        {
            case CardSearchTypes.Normal:
                return pCardList.Where(x => IsFoundInSearchableText(x, pSearchExp));
            case CardSearchTypes.Regex:
                return pCardList.Where(x => IsFoundInSearchableTextRegex(x, pSearchExp));
            default:
                Trace.WriteLine($"Search Type {pSearchType} not implemented.");
                return pCardList;
        }
    }

    private bool IsFoundInSearchableText(CardListingModel pCard, string pSearchText)
    {
        foreach (string text in pCard.SearchableText)
        {
            if (text.Contains(pSearchText, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsFoundInSearchableTextRegex(CardListingModel pCard, string pSearchRegex)
    {
        foreach (string text in pCard.SearchableText)
        {
            if (Regex.IsMatch(text, pSearchRegex, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private IQueryable<CardListingModel> FilterOnMisc(IQueryable<CardListingModel> pCardList, CardListFilter pFilters)
    {
        IQueryable<CardListingModel> filteredList = FilterSpecifics(pCardList, pFilters.CostFilters, ApplyCostFilter);
        filteredList = FilterSpecifics(filteredList, pFilters.TypeFilters, ApplyTypeFilter);
        filteredList = FilterSpecifics(filteredList, pFilters.RarityFilters, ApplyRarityFilter);
        filteredList = FilterSpecifics(filteredList, pFilters.CollectableFilters, ApplyCollectibleFilter);
        IQueryable<CardListingModel> filterOnRegion1 = FilterSpecifics(filteredList, pFilters.RegionFilters, ApplyRegion1Filter);
        IQueryable<CardListingModel> filterOnRegion2 = FilterSpecifics(filteredList, pFilters.RegionFilters, ApplyRegion2Filter);
        filteredList = filterOnRegion1.Union(filterOnRegion2);

        return filteredList;
    }

    private IQueryable<CardListingModel> FilterSpecifics<TKey>(
        IQueryable<CardListingModel> pCardList,
        FilterSet<TKey> pFilterSet,
        Func<CardListingModel, HashSet<TKey>, bool> pIsSatisfyFilter)
        where TKey : notnull, Enum
    {
        switch (pFilterSet.FilterType)
        {
            case FilterTypes.All:
                //  All enabled, so no changes.
                return pCardList;
            case FilterTypes.None:
                //  All disabled, so nothing.
                return Enumerable.Empty<CardListingModel>().AsQueryable();
            case FilterTypes.Specific:
                if (pFilterSet.SpecificFilters is null || pFilterSet.SpecificFilters.Count == 0)
                {
                    //  Treat is as if no filters are on.
                    return Enumerable.Empty<CardListingModel>().AsQueryable();
                }
                return pCardList.Where(x => pIsSatisfyFilter(x, pFilterSet.SpecificFilters));
            default:
                Trace.WriteLine($"Filter Type {pFilterSet.FilterType} not implemented.");
                return pCardList;
        }
    }

    private bool ApplyCostFilter(CardListingModel pCard, HashSet<CardFilterCosts> pFilters)
    {
        List<CardFilterCosts> costMap = [
            CardFilterCosts.Cost0, CardFilterCosts.Cost1, CardFilterCosts.Cost2,
            CardFilterCosts.Cost3, CardFilterCosts.Cost4, CardFilterCosts.Cost5,
            CardFilterCosts.Cost6, CardFilterCosts.Cost7, CardFilterCosts.Cost8,
            CardFilterCosts.Cost9, CardFilterCosts.Cost10Plus
        ];
        //  Bound negatives to 0, and 11+ to 10.
        int normCost = Math.Min(Math.Max(pCard.Cost, 0), 10);
        CardFilterCosts costType = costMap[normCost];
        return pFilters.Contains(costType);
    }

    private bool ApplyTypeFilter(CardListingModel pCard, HashSet<CardFilterTypes> pFilters) {
        switch (pCard.CardType)
        {
            case CardTypes.Ability:
                return pFilters.Contains(CardFilterTypes.Ability);
            case CardTypes.Champion:
                return pFilters.Contains(CardFilterTypes.Champion);
            case CardTypes.Equipment:
                return pFilters.Contains(CardFilterTypes.Equipment);
            case CardTypes.Landmark:
                return pFilters.Contains(CardFilterTypes.Landmark);
            case CardTypes.Spell:
                return pFilters.Contains(CardFilterTypes.Spell);
            case CardTypes.Trap:
                return pFilters.Contains(CardFilterTypes.Trap);
            case CardTypes.Unit:
                return pFilters.Contains(CardFilterTypes.Unit);
            default:
                Trace.WriteLine($"Card Type {pCard.CardType} not implemented for filtering.");
                return false;
        }
    }

    private bool ApplyRarityFilter(CardListingModel pCard, HashSet<CardFilterRarities> pFilters) {
        switch (pCard.CardRarity)
        {
            case CardRarities.None:
                return pFilters.Contains(CardFilterRarities.None);
            case CardRarities.Common:
                return pFilters.Contains(CardFilterRarities.Common);
            case CardRarities.Rare:
                return pFilters.Contains(CardFilterRarities.Rare);
            case CardRarities.Epic:
                return pFilters.Contains(CardFilterRarities.Epic);
            case CardRarities.Champion:
                return pFilters.Contains(CardFilterRarities.Champion);
            default:
                Trace.WriteLine($"Card Rarity {pCard.CardRarity} not implemented for filtering.");
                return false;
        }
    }

    private bool ApplyCollectibleFilter(CardListingModel pCard, HashSet<CardFilterCollectable> pFilters)
    {
        if (pCard.IsCollectible)
        {
            return pFilters.Contains(CardFilterCollectable.Collectable);
        }
        return pFilters.Contains(CardFilterCollectable.NonCollectable);
    }

    private bool ApplyRegion1Filter(CardListingModel pCard, HashSet<CardFilterRegions> pFilters)
    {
        return ApplyRegionFilter(pCard.Region1, pFilters);
    }

    private bool ApplyRegion2Filter(CardListingModel pCard, HashSet<CardFilterRegions> pFilters) {
        return ApplyRegionFilter(pCard.Region1, pFilters);
    }

    private bool ApplyRegionFilter(string pRegion, HashSet<CardFilterRegions> pFilters) {
        switch (pRegion.ToUpper())
        {
            case "NOXUS":
                return pFilters.Contains(CardFilterRegions.Noxus);
            case "DEMACIA":
                return pFilters.Contains(CardFilterRegions.Demacia);
            case "FRELJORD":
                return pFilters.Contains(CardFilterRegions.Freljord);
            case "SHADOWISLES":
                return pFilters.Contains(CardFilterRegions.ShadowIsles);
            case "TARGON":
                return pFilters.Contains(CardFilterRegions.Targon);
            case "IONIA":
                return pFilters.Contains(CardFilterRegions.Ionia);
            case "SHURIMA":
                return pFilters.Contains(CardFilterRegions.Shurima);
            case "BILGEWATER":
                return pFilters.Contains(CardFilterRegions.Bilgewater);
            case "PILTOVERZAUN":
                return pFilters.Contains(CardFilterRegions.PiltoverZaun);
            case "BANDLECITY":
                return pFilters.Contains(CardFilterRegions.BandleCity);
            default:
                Trace.WriteLine($"Card Region {pRegion} not implemented for filtering.");
                return false;
        }
    }

    private IQueryable<CardListingModel> SortList(IQueryable<CardListingModel> pCardList, CardListFilter pFilters)
    {
        bool isAsc = pFilters.SortOrder == CardSortOrders.Asc;
        IOrderedQueryable<CardListingModel> orderedList = (IOrderedQueryable<CardListingModel>)pCardList;

        switch (pFilters.SortType)
        {
            case CardSortTypes.Cost:
                if (isAsc)
                {
                    orderedList = pCardList.OrderBy(x => x.Cost);
                }
                else
                {
                    orderedList = pCardList.OrderByDescending(x => x.Cost);
                }
                break;
            case CardSortTypes.Name:
                if (isAsc)
                {
                    orderedList = pCardList.OrderBy(x => x.Name);
                }
                else
                {
                    orderedList = pCardList.OrderByDescending(x => x.Name);
                }
                break;
            case CardSortTypes.Rarity:
                if (isAsc)
                {
                    orderedList = pCardList.OrderBy(x => x.CardRarity.ToString());
                }
                else
                {
                    orderedList = pCardList.OrderByDescending(x => x.CardRarity.ToString());
                }
                break;
            case CardSortTypes.Region:
                if (isAsc)
                {
                    orderedList = pCardList.OrderBy(x => x.Region1).ThenBy(x => x.Region2);
                }
                else
                {
                    orderedList = pCardList.OrderByDescending(x => x.Region1).ThenByDescending(x => x.Region2);
                }
                break;
            case CardSortTypes.Type:
                if (isAsc)
                {
                    orderedList = pCardList.OrderBy(x => x.CardType);
                }
                else
                {
                    orderedList = pCardList.OrderByDescending(x => x.CardType);
                }
                break;
            default:
                Trace.WriteLine($"Sort type {pFilters.SortType} not implemented.");
                return pCardList;
        }

        if (isAsc)
        {
            return orderedList.ThenBy(x => x.Name).ThenBy(x => x.Cost).ThenBy(x => x.CardType).ThenBy(x => x.CardRarity);
        }
        else
        {
            return orderedList.ThenByDescending(x => x.Name).ThenByDescending(x => x.Cost).ThenByDescending(x => x.CardType).ThenByDescending(x => x.CardRarity);
        }
    }
}
