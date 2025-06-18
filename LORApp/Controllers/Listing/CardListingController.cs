using LORApp.Models.Cards;
using LORApp.Models.Listing;

namespace LORApp.Controllers.Listing;

internal class CardListingController : IListingController
{
  #region IListingController
  public CardListModel CreateCardList(IEnumerable<CardModel> pCardList)
  {
    return new()
    {
      CardListing = pCardList.Select(x => CreateCardListing(x))
    };
  }

  public CardListingModel CreateCardListing(CardModel pCard)
  {
    return new()
    {
      CardCode = pCard.CardCode,
      Name = pCard.Name,
      Cost = pCard.Cost,
      CardType = pCard.Type.ToString(),
      Rarity = pCard.Rarity.ToString(),
      RegionIcons = pCard.RegionRefs.Select(x => x.IconPath),
      IsFavorited = false
    };
  }
  #endregion
}