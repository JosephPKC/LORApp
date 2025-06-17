using CommunityToolkit.Mvvm.ComponentModel;

using LORApp.Controllers.Cards;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Cards;

internal abstract class BaseCardViewModel<TCard>(TCard pCard, ICardController<TCard> pController) :
    ObservableObject, IQueryAttributable 
    where TCard : CardModel
{
    protected readonly ICardController<TCard> _controller = pController;
    protected TCard _card = pCard;

    #region Properties
    public virtual string Name => _card.Name;
    public virtual string Description => _card.Description;
    public virtual string ArtUriPath => _card.ArtUriPath;
    public virtual int Cost => _card.Cost;

    public virtual IEnumerable<string> Keywords => _card.KeywordRefs;
    #endregion

    #region Abstracts
    protected abstract void LoadCard(TCard pCard);
    protected abstract void RefreshProperties();
    #endregion

    #region IQueryAttributable
    public virtual void ApplyQueryAttributes(IDictionary<string, object> pQuery)
    {
        //  Try to load the card based on the code query.
        if (!pQuery.TryGetValue("card", out object? val))
        {
            //  Possibly throw an error?
            return;
        }

        string? cardCode = val?.ToString();
        if (cardCode is null)
        {
            //  Possibly throw an error?
            return;
        }

        TCard? card = _controller.LoadCard(cardCode);
        if (card is not null)
        {
            LoadCard(card);
            RefreshProperties();
        }
    }
    #endregion
}
