namespace LORApp.Services.Repo;

internal static class CardRepositoryFactory
{
  public static ICardRepository CreateCardRepo()
  {
    return new CardRepository();
  }
}