using LORApp.Entities.Listing;

namespace LORApp.Services.CardRepo.Caching;

/// <summary>
/// Cache specifically for the card listing.
/// </summary>
internal class CardListCache
{
    private CardListModel? _cardList;

    public CardListModel? GetCardList()
    {
        return _cardList;
    }

    public void LoadCardList(CardListModel pCardList)
    {
        _cardList = pCardList;
    }
}
