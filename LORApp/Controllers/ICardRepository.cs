using LORApp.Models.Cards;

namespace LORApp.Controllers;

internal interface ICardRepository
{
  TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel;
}