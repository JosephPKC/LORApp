using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal class NullCardController : BaseCardController, ICardController<CardModel>
{
  #region ICardController<CardModel>
  public CardModel? LoadCard(string pCardCode)
  {
    return null;
  }
  #endregion
}