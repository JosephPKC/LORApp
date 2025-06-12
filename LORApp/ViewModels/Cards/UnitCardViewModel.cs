using CommunityToolkit.Mvvm.ComponentModel;
using LORApp.Models.Cards;
using System.Diagnostics;

namespace LORApp.ViewModels.Cards;

internal partial class UnitCardViewModel : ObservableObject, IQueryAttributable
{
    private UnitCardModel _card;

    #region Properties
    public string Name => _card.Name;
    #endregion

    public UnitCardViewModel()
    {
        _card = new();
    }

    public UnitCardViewModel(UnitCardModel pCard)
    {
        _card = pCard;
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
        UnitCardModel? card = CardController.LoadUnitCard(cardCode);
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
