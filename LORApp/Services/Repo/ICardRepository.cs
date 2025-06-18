using LORApp.Models.Cards;

namespace LORApp.Services.Repo;

internal interface ICardRepository
{
  IEnumerable<CardModel> FetchAll();
  TCard? FetchCard<TCard>(string pCardCode) where TCard : CardModel;
}