namespace LORApp.Models.Listing;

internal class CardListingModel : BaseModel
{
    public string CardCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; } = 0;
    public string CardType { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public IEnumerable<string> RegionIcons { get; set; } = [];
    public bool IsFavorited { get; set; } = false;
}
