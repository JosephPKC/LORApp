using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Listing;

internal partial class ListingViewModel : ObservableObject
{
    public ObservableCollection<CardModel> CardListing => [new UnitCardModel() { CardCode = "123", Name = "Test Card 1", Type = CardTypes.Unit}, new UnitCardModel() { CardCode = "12345", Name = "Test Card 2", Type = CardTypes.Unit }];
    public ICommand SelectCardCommand { get; }

    public ListingViewModel()
    {
        SelectCardCommand = new AsyncRelayCommand<CardModel>(SelectCardAsync);
    }

    #region Commands
    private async Task SelectCardAsync(CardModel? pCard)
    {
        Trace.WriteLine("SELECT CARD: " + pCard);
        if (pCard is null)
        {
            return;
        }

        //  Go to the correct page based on the card type
        string cardPagePath = pCard.Type switch
        {
            CardTypes.Champion => $"{nameof(Views.Cards.UnitCardPage)}",
            CardTypes.Spell => $"{nameof(Views.Cards.UnitCardPage)}",
            CardTypes.Unit => $"{nameof(Views.Cards.UnitCardPage)}",
            _ => string.Empty
        };

        if (!string.IsNullOrWhiteSpace(cardPagePath))
        {
            cardPagePath += $"?card={pCard.CardCode}";
            await Shell.Current.GoToAsync(cardPagePath);
        }
    }
    #endregion

}
