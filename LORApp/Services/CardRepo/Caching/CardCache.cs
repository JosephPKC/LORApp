using LORApp.Entities.Cards;

namespace LORApp.Services.CardRepo.Caching;

internal enum CardHitTypes
{
    Found,
    Missing,
    Null
}

internal class CardCacheResult<TCard> where TCard : CardModel
{
    public CardHitTypes Type { get; set; }
    public TCard? CardModel { get; set; }
}

/// <summary>
/// A cache for cards
/// </summary>
internal class CardCache
{
    private readonly Dictionary<string, CardModel?> _cardCache = [];

    public CardCacheResult<TCard> GetCard<TCard>(string pCardCode) where TCard : CardModel
    {
        if (string.IsNullOrWhiteSpace(pCardCode))
        {
            return new()
            {
                Type = CardHitTypes.Missing,
                CardModel = null
            };
        }

        if (!_cardCache.TryGetValue(pCardCode, out CardModel? value))
        {
            return new()
            {
                Type = CardHitTypes.Missing,
                CardModel = null
            };
        }

        if (value is null)
        {
            return new()
            {
                Type = CardHitTypes.Null,
                CardModel = null
            };
        }

        return new()
        {
            Type = CardHitTypes.Found,
            CardModel = value as TCard
        };
    }

    public void PutCard<TCard>(string pCardCode, TCard? pCard) where TCard : CardModel
    {
        if (string.IsNullOrWhiteSpace(pCardCode))
        {
            return;
        }

        if (_cardCache.ContainsKey(pCardCode))
        {
            _cardCache[pCardCode] = pCard;
        }
        else
        {
            _cardCache.Add(pCardCode, pCard);
        }
    }

    public void Clear()
    {
        _cardCache.Clear();
    }
}
