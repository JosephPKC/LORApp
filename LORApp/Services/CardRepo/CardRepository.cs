using System.Data;

using LORApp.Entities.Cards;
using LORApp.Entities.Listing;
using LORApp.Services.CardRepo.Mappers;
using LORApp.UseCases.Services;
using LORApp.Utils.Configs;

namespace LORApp.Services.CardRepo;

internal class CardRepository : ICardGateway
{
    private readonly IRepo _repo;
    private readonly ICardMapperManager _mapper;

    public CardRepository(IRepo pRepo, ICardMapperManager pMapper)
    {
        _repo = pRepo;
        _mapper = pMapper;
    }

    #region ICardGateway
    public TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel
    {
        //DataTable dt = _repo.Read(ConfigManager.CardTable, null, [new("CardCode", pCardCode)]);
        //return _mapper.MapCard<TCard>(dt);
        if (cards.ContainsKey(pCardCode.ToUpper()))
        {
            return cards[pCardCode.ToUpper()] as TCard;
        }

        return default;
    }

    public CardListModel FetchCardList()
    {
        //DataTable dt = _repo.Read(ConfigManager.CardListingTable, null, null);
        //return _mapper.MapCardList(dt);
        return new()
        {
            CardListing = cardListing
        };
    }
    #endregion


    private Dictionary<string, CardModel> cards = new(){
        { 
            "UNITCODE1", 
            new UnitCardModel() {
                Name = "Unit Card 1",
                Artist = "Unit Artist",
                Attack = 3,
                Health = 2,
                Cost = 1,
                Description = "Unit Description...",
                FlavorText = "~Flavor Text~",
                Rarity = CardRarities.Rare,
                CardCode = "UNITCODE1"
            } 
        },
        { 
            "SPELLCODE2", 
            new SpellCardModel() {
                Name = "Spell Card 2",
                Artist = "Artiste1",
                SpellSpeed = SpellSpeeds.Fast,
                Cost = 2,
                Description = "Spell Description",
                FlavorText = "~Flavor",
                Rarity = CardRarities.Common,
                CardCode = "SPELLCODE2"
            } 
        },
        { 
            "CHAMPIONCODE3", 
            new ChampionCardModel() {
                Name = "The Champion",
                Artist = "ART",
                Attack = 10,
                Health = 1,
                Cost = 6,
                Description = "THE CHAMPION",
                FlavorText = "~flavote",
                Rarity = CardRarities.Champion,
                CardCode = "CHAMPIONCODE3"
            } 
        }
    };

    private CardListingModel[] cardListing =
    {
        new CardListingModel()
        {
            Name = "Unit Card 1",
            CardCode = "UNITCODE1",
            CardRarity = CardRarities.Rare,
            CardType = CardTypes.Unit,
            Cost = 1,
            IsFavorite = true
        },
        new CardListingModel()
        {
            Name = "Spell Card 2",
            CardCode = "SPELLCODE2",
            CardRarity = CardRarities.Common,
            CardType = CardTypes.Spell,
            Cost = 2,
            IsFavorite = false
        },
        new CardListingModel()
        {
            Name = "The Champion",
            CardCode = "CHAMPIONCODE3",
            CardRarity = CardRarities.Champion,
            CardType = CardTypes.Champion,
            Cost = 6,
            IsFavorite = false
        },
    };
}
