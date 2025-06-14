namespace LORApp.Models.Cards;

internal enum SpellSpeeds {
  Slow,
  Fast,
  Burst
}

internal class SpellCardModel : CardModel
{
  public override CardTypes Type { get; set; } = CardTypes.Spell;
  public SpellSpeeds SpellSpeed { get; set; } = SpellSpeeds.Slow;
}