using LORApp.Entities.Refs;

namespace LORApp.Entities.Cards;

public enum CardRarities
{
    None,
    Common,
    Rare,
    Epic,
    Champion
}

public enum CardTypes
{
    Champion,
    Spell,
    Unit
}

public abstract class CardModel : BaseModel
{
    public abstract CardTypes Type { get; set; }
    public virtual CardRarities Rarity { get; set; } = CardRarities.Common;

    public string CardCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FlavorText { get; set; } = string.Empty;
    public string ArtUriPath { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Cost { get; set; } = 0;

    public IEnumerable<string> KeywordRefs { get; set; } = [];
    public IEnumerable<RegionModel> RegionRefs { get; set; } = [];
    public IEnumerable<string> SubTypes { get; set; } = [];
    public string SuperType { get; set; } = string.Empty;
    public IEnumerable<string> AssociatedCardCodes { get; set; } = [];
}
