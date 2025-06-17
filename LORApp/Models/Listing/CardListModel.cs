namespace LORApp.Models.Listing;

internal class CardListModel : BaseModel
{
    public IEnumerable<CardListingModel> CardListing { get; set; } = [];
}
