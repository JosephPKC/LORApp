namespace LORCardLoader.Models;

internal class CardModel : BaseModel
{
    public IEnumerable<string> AssociatedCards { get; set; } = [];
    public IEnumerable<string> AssociatedCardRefs { get; set; } = [];
    public IEnumerable<AssetModel> Assets { get; set; } = [];
    public IEnumerable<string> Regions { get; set; } = [];
    public IEnumerable<string> RegionRefs { get; set; } = [];
    public int Attack { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Health { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public string DescriptionRaw { get; set; } = string.Empty;
    public string LevelUpDescription { get; set; } = string.Empty;
    public string LevelUpDescriptionRaw { get; set; } = string.Empty;
    public string FlavorText { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CardCode { get; set; } = string.Empty;
    public IEnumerable<string> Keywords { get; set; } = [];
    public IEnumerable<string> KeywordRefs { get; set; } = [];
    public string SpellSpeed { get; set; } = string.Empty;
    public string SpellSpeedRef { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public string RarityRef { get; set; } = string.Empty;
    public IEnumerable<string> Subtypes { get; set; } = [];
    public string Supertype { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Collectible { get; set; } = false;
    public string Set { get; set; } = string.Empty;
    public IEnumerable<string> Formats { get; set; } = [];
    public IEnumerable<string> FormatRefs { get; set; } = [];
}
