using LORApp.Entities.Listing;
using LORApp.UseCases.Services;
using System.Diagnostics;

namespace LORApp.UseCases.Listing;

internal class ListCardsUseCase : IListCards
{
    private readonly ICardGateway _cardGateway;

    public ListCardsUseCase(ICardGateway pCardGateway)
    {
        _cardGateway = pCardGateway;
    }

    #region IListCards
    public CardListModel LoadCardList()
    {
        Trace.WriteLine("LOADING CARD LIST");
        return _cardGateway.FetchCardList();
    }
    #endregion
}
