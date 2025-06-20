using LORApp.Entities.Cards;
using LORApp.Entities.Listing;

namespace LORApp.UseCases.Services;

public interface ICardGateway
{
    TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel;
    CardListModel FetchCardList();
}
