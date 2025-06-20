using LORApp.Entities.Cards;

namespace LORApp.UseCases.Cards;

public interface ILoadCard
{
    TCard? LoadCard<TCard>(string pCardCode) where TCard : CardModel;
}
