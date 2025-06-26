using System.Data;
using System.Diagnostics;

using LORApp.Entities.Cards;
using LORApp.Entities.Listing;
using LORApp.Entities.Refs;
using LORApp.Services.CardRepo.Caching;
using LORApp.Services.CardRepo.Mappers;
using LORApp.UseCases.Services;
using LORApp.Utils.Configs;

namespace LORApp.Services.CardRepo;

internal class CardRepository : ICardGateway
{
    private readonly IRepo _repo;
    private readonly CardCache _cardCache;
    private readonly CardListCache _cardListCache;
    private readonly RefCache _refCache;

    public CardRepository(IRepo pRepo)
    {
        _repo = pRepo;
        _cardCache = new();
        _cardListCache = new();
        DataTable dt = _repo.Read("Keywords", null, null);
        IEnumerable<KeywordModel> keywords = RefMappers.MapKeywordList(dt);
        dt = _repo.Read("Vocab", null, null);
        IEnumerable<KeywordModel> vocab = RefMappers.MapKeywordList(dt);
        dt = _repo.Read("Regions", null, null);
        IEnumerable<RegionModel> regions = RefMappers.MapRegionList(dt);
        _refCache = new(keywords, vocab, regions);
    }

    #region ICardGateway
    public TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel
    {
        if (string.IsNullOrWhiteSpace(pCardCode))
        {
            Trace.WriteLine($"CardCode is blank.");
            return null;
        }

        CardCacheResult<TCard> cardResult = _cardCache.GetCard<TCard>(pCardCode);
        if (cardResult.Type == CardHitTypes.Found) {
            return cardResult.CardModel;
        }

        if (cardResult.Type == CardHitTypes.Null)
        {
            Trace.WriteLine($"{pCardCode} has a null cache result.");
            return null;
        }

        //  Retrieve and build the card model
        DataTable cardDt = _repo.Read(ConfigManager.CardTable, null, [new("CardCode", pCardCode)]);

        //  Get the base card model
        TCard? card = CardMappers.MapCard<TCard>(cardDt);
        if (card is null)
        {
            //  Note, we are caching a null result to avoid redoing the operations.
            //  Store the null result, so that we know that the card code is invalid or incorrect.
            _cardCache.PutCard(pCardCode, card);
            Trace.WriteLine($"{pCardCode} is invalid in some way.");
            return null;
        }

        //  Append any specific card type info
        switch (typeof(TCard).Name)
        {
            case nameof(ChampionCardModel):
                DataTable champDt = _repo.Read(ConfigManager.ChampionCardTable, null, [new("CardCode", pCardCode)]);
                CardMappers.AppendChampionCardInfo((card as ChampionCardModel)!, champDt);
                break;
            case nameof(UnitCardModel):
                DataTable unitDt = _repo.Read(ConfigManager.UnitCardTable, null, [new("CardCode", pCardCode)]);
                CardMappers.AppendUnitCardInfo((card as UnitCardModel)!, unitDt);
                break;
        }

        //  Get and append card links
        DataTable assocCardDt = _repo.Read(ConfigManager.CardAssocCardLinkTable, null, [new("CardCode", pCardCode)]);
        CardMappers.AppendAssociatedCards(card, assocCardDt);
        DataTable keywordDt = _repo.Read(ConfigManager.CardKeywordLinkTable, null, [new("CardCode", pCardCode)]);
        CardMappers.AppendCardKeywords(card, keywordDt, _refCache);
        DataTable regionDt = _repo.Read(ConfigManager.CardRegionLinkTable, null, [new("CardCode", pCardCode)]);
        CardMappers.AppendCardRegions(card, regionDt, _refCache);
        DataTable subTypeDt = _repo.Read(ConfigManager.CardSubtypeLinkTable, null, [new("CardCode", pCardCode)]);
        CardMappers.AppendCardSubtypes(card, subTypeDt);

        _cardCache.PutCard(pCardCode, card);
        return card;
    }

    public CardListModel FetchCardList()
    {
        CardListModel? cardListFromCache = _cardListCache.GetCardList();
        if (cardListFromCache is not null)
        {
            return cardListFromCache;
        }

        //  Retrieve and build card list
        DataTable allCardsDt = _repo.Read(ConfigManager.CardTable, null, null);
        CardListModel cardList = CardListMapper.MapCardList(allCardsDt);
        
        foreach (CardListingModel cardListing in cardList.CardListing)
        {
            DataTable regionDt = _repo.Read(ConfigManager.CardRegionLinkTable, null, [new("CardCode", cardListing.CardCode)]);
            CardListMapper.AppendCardListingRegion(cardListing, regionDt, _refCache);
        }

        _cardListCache.LoadCardList(cardList);
        return cardList;
    }
    #endregion
}
