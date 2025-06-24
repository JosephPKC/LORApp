namespace LORApp.Entities.Cards;

public class ChampionCardModel : UnitCardModel
{
    public override CardTypes Type { get; set; } = CardTypes.Champion;
    public override CardRarities Rarity { get; set; } = CardRarities.Champion;
    public string LevelUpDescription { get; set; } = string.Empty;
    public string LevelUpDescriptionFormatted { get; set; } = string.Empty;
    public string LevelUpChamionCode { get; set; } = string.Empty; // Maybe it can be a class ref instead.
}
