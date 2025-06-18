using LORApp.Models.Cards;

namespace LORApp.Services.Repo;

internal class CardRepository : ICardRepository
{
    #region ICardRepository
    public IEnumerable<CardModel> FetchAll()
    {
        return [
            new ChampionCardModel()
        ];
    }

    public TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel
    {
        return nameof(TCard) switch
        {
            nameof(ChampionCardModel) => new ChampionCardModel() as TCard,
            nameof(SpellCardModel) => new SpellCardModel() as TCard,
            nameof(UnitCardModel) => new UnitCardModel() as TCard,
            _ => default
        };
    }
    #endregion
}
