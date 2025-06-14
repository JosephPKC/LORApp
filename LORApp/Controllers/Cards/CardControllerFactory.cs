using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal static class CardControllerFactory
{
  public static ICardController<TCard> CreateController<TCard>() where TCard : CardModel
  {
    return nameof(TCard) switch
    {
      nameof(ChampionCardModel) => (ICardController<TCard>)new ChampionCardController(),
      nameof(SpellCardModel) => (ICardController<TCard>)new SpellCardController(),
      nameof(UnitCardModel) => (ICardController<TCard>)new UnitCardController(),
      _ => (ICardController<TCard>)new NullCardController()
    };
  }
}