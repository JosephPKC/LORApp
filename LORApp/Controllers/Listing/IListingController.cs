using LORApp.Models.Cards;
using LORApp.Models.Listing;

namespace LORApp.Controllers.Listing;

internal interface IListingController
{
  CardListModel CreateCardList(IEnumerable<CardModel> pCardList);
  CardListingModel CreateCardListing(CardModel pCard);
}