using LORApp.Entities.Cards;
using LORApp.UseCases.Services;

namespace LORApp.UseCases.Cards;

internal class LoadCardUseCase : ILoadCard
{
    private readonly ICardGateway _cardGateway;

    public LoadCardUseCase(ICardGateway pCardGateway)
    {
        _cardGateway = pCardGateway;
    }

    #region ILoadCard
    public TCard? LoadCard<TCard>(string pCardCode) where TCard : CardModel
    {
        return _cardGateway.FetchCard<TCard>(pCardCode);
    }
    #endregion
}
