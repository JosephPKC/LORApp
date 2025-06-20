namespace LORApp.Entities.Listing;

public class CardListModel : BaseModel
{
    public IEnumerable<CardListingModel> CardListing { get; set; } = [];
}
