using LORApp.Controllers.Cards;
using LORApp.Controllers.Listing;
using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.Controllers;

internal static class ControllerFactory
{
  public static ICardController<TCard> CreateCardController<TCard>(ICardRepository pRepo) where TCard : CardModel
  {
    return nameof(TCard) switch
    {
      nameof(ChampionCardModel) => (ICardController<TCard>)new ChampionCardController(pRepo),
      nameof(SpellCardModel) => (ICardController<TCard>)new SpellCardController(pRepo),
      nameof(UnitCardModel) => (ICardController<TCard>)new UnitCardController(pRepo),
      _ => (ICardController<TCard>)new NullCardController(pRepo)
    };
  }

  public static IListingController CreateListingController()
  {
    return new CardListingController();
  }
}