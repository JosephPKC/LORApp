using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal class ChampionCardController(ICardRepository pCardRepo) : BaseCardController(pCardRepo), ICardController<ChampionCardModel>
{
  #region ICardController<ChampionCardModel>
  public ChampionCardModel? LoadCard(string pCardCode)
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