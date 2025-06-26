namespace LORApp.Entities.Cards;

internal class AbilityCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Ability;
}

internal class EquipmentCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Equipment;
}

internal class LandmarkCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Landmark;
}

internal class SpellCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Spell;
}

internal class TrapCardModel : CardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Trap;
}