using LORApp.Models.Cards;

namespace LORApp.Controllers.Cards;

internal interface ICardController<TCard> where TCard : CardModel
{
  TCard? LoadCard(string pCardCode);
}