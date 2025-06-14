namespace LORApp.Models.Cards;

internal class ChampionCardModel : CardModel
{
  public override CardTypes Type { get; set; } = CardTypes.Champion;
  public string LevelUpDescription { get; set; } = string.Empty;

  // Associated card codes
  // The idea is that the controller can do lookup based on codes.
  // The view model can hold references instead.
  // Each champion that levels up needs a level up code. This chains if there are multiple level ups.
  // Associated cards are for associated spells and tokens.
  public string LevelUpChamionCode { get; set; } = string.Empty; // Maybe it can be a class ref instead.
  public IEnumerable<string> AssociatedCardCodes { get; set; } = [];
}
