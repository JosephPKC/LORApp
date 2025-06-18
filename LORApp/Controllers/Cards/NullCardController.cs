using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.Controllers.Cards;

internal class NullCardController(ICardRepository pCardRepo) : BaseCardController(pCardRepo), ICardController<CardModel>
{
  #region ICardController<CardModel>
  public CardModel? LoadCard(string pCardCode)
  {
    return null;
  }
  #endregion
}