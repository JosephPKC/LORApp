using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.Controllers.Cards;

internal class SpellCardController(ICardRepository pCardRepo) : BaseCardController(pCardRepo), ICardController<SpellCardModel>
{
  #region ICardController<SpellCardModel>
  public SpellCardModel? LoadCard(string pCardCode)
  {
    //  This will need to do a query to the sqlite db to get the card model.
    //  For now, just return a dummy.
    return new()
    {
      CardCode = pCardCode,
      Name = "Test Card"
    };
  }
  #endregion
}