using System.Data;

using LORApp.Entities.Cards;
using LORApp.Entities.Listing;

namespace LORApp.Services.CardRepo.Mappers;

public interface ICardMapperManager
{
    TCard? MapCard<TCard>(DataTable pDt) where TCard : CardModel;

    CardListModel MapCardList(DataTable pDt);
}
