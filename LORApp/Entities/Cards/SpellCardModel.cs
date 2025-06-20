namespace LORApp.Entities.Cards;

public enum SpellSpeeds
{
    Slow,
    Fast,
    Burst
}

public class SpellCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Spell;
    public SpellSpeeds SpellSpeed { get; set; } = SpellSpeeds.Slow;
}
