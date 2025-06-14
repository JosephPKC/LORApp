using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

using LORApp.Controllers.Cards;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Cards;

internal partial class UnitCardViewModel : ObservableObject, IQueryAttributable
{
    private UnitCardModel _card;
    private ICardController<UnitCardModel> _controller;

    #region Properties
    public string Name => _card.Name;
    #endregion

    public UnitCardViewModel()
    {
        _card = new();
        _controller = CardControllerFactory.CreateController<UnitCardModel>();
    }

    public UnitCardViewModel(UnitCardModel pCard)
    {
        _card = pCard;
        _controller = CardControllerFactory.CreateController<UnitCardModel>();
    }

    #region IQueryAttributable
    public void ApplyQueryAttributes(IDictionary<string, object> pQuery)
    {
        Trace.WriteLine("Loading card page...");
        //  Try to load the card based on the card code query, if it exists.
        //  If not, then there is nothing to do on this page.
        if (!pQuery.TryGetValue("card", out object? val))
        {
            return;
        }

        string? cardCode = val?.ToString();
        if (cardCode is null)
        {
            return;
        }

        Trace.WriteLine("Loading card " + cardCode);
        UnitCardModel? card = _controller.LoadCard(cardCode);
        if (card is not null)
        {
            _card = card;
            RefreshProperties();
        }
    }
    #endregion

    #region Commands

    #endregion

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
    }
}
