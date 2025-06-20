using CommunityToolkit.Mvvm.ComponentModel;
using LORApp.Entities.Cards;
using LORApp.UseCases.Cards;

namespace LORApp.Views.Cards;

public abstract class BaseCardViewModel<TCard> : ObservableObject, IQueryAttributable where TCard : CardModel
{
    protected TCard _card;
    protected readonly ILoadCard _loadCard;

    public BaseCardViewModel(ILoadCard pLoadCard)
    {
        _card = (TCard)(CardModel)new UnitCardModel();
        _loadCard = pLoadCard;
    }

    #region Properties
    public virtual string Name => _card.Name;
    public virtual string Description => _card.Description;
    public virtual string ArtUriPath => _card.ArtUriPath;
    public virtual int Cost => _card.Cost;

    public virtual IEnumerable<string> Keywords => _card.KeywordRefs;
    #endregion

    #region Abstracts & Virtuals
    protected virtual void RefreshProperties()
    {

    }
    #endregion

    #region IQueryAttributable
    public virtual void ApplyQueryAttributes(IDictionary<string, object> pQuery)
    {
        if (!pQuery.TryGetValue("card", out object? val))
        {
            //  Possibly throw an error?
            return;
        }

        string? cardCode = (string)val;
        if (cardCode is null)
        {
            //  Possibly throw an error?
            return;
        }

        TCard? card = _loadCard.LoadCard<TCard>(cardCode);
        if (card is not null)
        {
            _card = card;
            RefreshProperties();
        }
    }
    #endregion
}
