using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal static class CardControllerFactory
{
  public static ICardController<TCard> CreateController<TCard>(ICardRepository pRepo) where TCard : CardModel
  {
    return nameof(TCard) switch
    {
      nameof(ChampionCardModel) => (ICardController<TCard>)new ChampionCardController(pRepo),
      nameof(SpellCardModel) => (ICardController<TCard>)new SpellCardController(pRepo),
      nameof(UnitCardModel) => (ICardController<TCard>)new UnitCardController(pRepo),
      _ => (ICardController<TCard>)new NullCardController(pRepo)
    };
  }
}