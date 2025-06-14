using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal abstract class BaseCardController(ICardRepository pCardRepo)
{
  protected ICardRepository _repo = pCardRepo;
  public TCard? LoadCard<TCard>(string pCardCode) where TCard : CardModel
  {
    return _repo.FetchCard<TCard>(pCardCode);
  }
}