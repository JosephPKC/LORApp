using System.Data;
using LORApp.Entities.Cards;
using LORApp.Entities.Listing;

namespace LORApp.Services.CardRepo.Mappers;

internal class CardMapperManager : ICardMapperManager
{
    #region ICardMapperManager
    public TCard? MapCard<TCard>(DataTable pDt) where TCard : CardModel
    {
        throw new NotImplementedException();
    }

    public CardListModel MapCardList(DataTable pDt)
    {
        return CardListMapper.MapCardList(pDt);
    }
    #endregion
}
