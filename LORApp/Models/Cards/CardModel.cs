namespace LORApp.Models.Cards;

internal enum CardRarities
{
    Common,
    Uncommon,
    Rare,
    Champion
}

internal enum CardTypes
{
    Spell,
    Unit
}

internal abstract class CardModel : BaseModel
{
    public string CardCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FlavorText { get; set; } = string.Empty;
    public string ArtUriPath { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Cost { get; set; } = 0;
    public CardRarities Rarity { get; set; } = CardRarities.Common;
    public CardTypes Type { get; set; } = CardTypes.Spell;
}
