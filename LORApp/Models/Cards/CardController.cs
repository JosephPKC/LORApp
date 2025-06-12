namespace LORApp.Models.Cards;

internal static class CardController
{
    public static UnitCardModel? LoadUnitCard(string pCardCode)
    {
        //  This will need to do a query to the sqlite db to get the card model.
        //  For now, just return a dummy.
        return new()
        {
            CardCode = pCardCode,
            Name = "Test Card"
        };
    }
}
