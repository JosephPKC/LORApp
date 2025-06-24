using LORApp.Entities.Cards;

namespace LORApp.Entities.Listing;

public class CardListingModel : BaseModel
{
    public string CardCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; } = 0;
    public CardTypes CardType { get; set; } = CardTypes.Unit;
    public CardRarities CardRarity { get; set; } = CardRarities.Common;
    public string RegionIcon1 { get; set; } = string.Empty;
    public string RegionIcon2 { get; set; } = string.Empty;
    public bool IsCollectible { get; set; } = false;
}
