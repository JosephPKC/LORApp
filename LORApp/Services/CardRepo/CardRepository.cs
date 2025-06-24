using System.Data;
using System.Collections.Generic;

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
    private readonly IRepoCache _cache;
    private readonly IModelMapperManager _mapper;


    // CACHE
    private readonly Dictionary<string, RegionModel> _regionCache = [];
    private readonly Dictionary<string, KeywordModel> _keywordCache = [];
    private readonly Dictionary<string, KeywordModel> _vocabCache = [];
    private readonly Dictionary<string, CardModel> _cardCache = [];
    private Dictionary<string, CardListingModel>? _cardListingCache = null;
    // ENDCACHE

    public CardRepository(IRepo pRepo, IRepoCache pCache, IModelMapperManager pMapper)
    {
        _repo = pRepo;
        _cache = pCache;
        _mapper = pMapper;

        DataTable regionDt = _repo.Read("Regions", null, null);
        IEnumerable<RegionModel> regions = _mapper.MapList<RegionModel>(regionDt);
        _regionCache = regions.ToDictionary(x => x.RefCode, x => x);

        DataTable keywordDt = _repo.Read("Keywords", null, null);
        IEnumerable<KeywordModel> keywords = _mapper.MapList<KeywordModel>(keywordDt);
        _keywordCache = keywords.ToDictionary(x => x.RefCode, x => x);

        DataTable vocabDt = _repo.Read("Vocab", null, null);
        IEnumerable<KeywordModel> vocab = _mapper.MapList<KeywordModel>(vocabDt);
        _vocabCache = vocab.ToDictionary(x => x.RefCode, x => x);
    }

    #region ICardGateway
    public TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel
    {
        if (_cardCache.ContainsKey(pCardCode)) {
            return _cardCache[pCardCode];
        }

        DataTable dt = _repo.Read(ConfigManager.CardTable, null, [new("CardCode", pCardCode)]);
        //  Get links

        TCard card = _mapper.MapCard<TCard>(dt);
        _cardCache.Add(pCardCode, card);
        return card;
    }

    public CardListModel FetchCardList()
    {
        if (_cardListingCache is not null)
        {
            return _cardListingCache;
        }

        DataTable dt = _repo.Read(ConfigManager.CardTable, null, null);
        //  Get card listing links

        IEnumerable<CardListingModel> cardListing = _mapper.MapCardList(dt);

        CardListModel cardList = new()
        {
            CardListing = cardListing
        };
        _cardListingCache = cardList;
        return cardList;
    }
    #endregion

    private void LoadCardList()
    {
        //  Get all Cards
        //  For each card:
        //      Get region links, keywords, subtypes.
        //      Create card listing
    }

    private void LoadCard()
    {
        //  Get Card info
        //  Get card's links (regions, keywords, subtypes, assoc cards)
        //  Parse enums from strings
        //  Get keywords/vocabs from card descriptions
        //  Load up the descriptions for keywords/vocabs
        //  Create card model
    }
    {
        new CardListingModel()
        {
            Name = "Unit Card 1",
            CardCode = "UNITCODE1",
            CardRarity = CardRarities.Rare,
            CardType = CardTypes.Unit,
            Cost = 1,
        },
        new CardListingModel()
        {
            Name = "Spell Card 2",
            CardCode = "SPELLCODE2",
            CardRarity = CardRarities.Common,
            CardType = CardTypes.Spell,
            Cost = 2,
        },
        new CardListingModel()
        {
            Name = "The Champion",
            CardCode = "CHAMPIONCODE3",
            CardRarity = CardRarities.Champion,
            CardType = CardTypes.Champion,
            Cost = 6,
        },
    };
}
