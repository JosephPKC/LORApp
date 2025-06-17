using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LORApp.Models.Cards;
using LORApp.Models.Listing;

namespace LORApp.ViewModels.Listing;

internal partial class ListingViewModel : ObservableObject
{
    private CardListModel _cardList;

    #region Properties
    public ObservableCollection<CardListingModel> CardListing { get; }
    #endregion

    #region Commands
    public ICommand SelectCardCommand { get; }
    #endregion

    public ListingViewModel()
    {
        _cardList = new();
        SelectCardCommand = new AsyncRelayCommand<CardModel>(SelectCardAsync);
        CardListing = [];
    }

    public ListingViewModel(CardListModel pCardList)
    {
        _cardList = pCardList;
        SelectCardCommand = new AsyncRelayCommand<CardModel>(SelectCardAsync);
        CardListing = (ObservableCollection<CardListingModel>)_cardList.CardListing;
    }

    #region Command Defs
    private async Task SelectCardAsync(CardModel? pCard)
    {
        if (pCard is null)
        {
            return;
        }

        //  Go to the correct page based on the card type
        string cardPagePath = pCard.Type switch
        {
            CardTypes.Champion => $"{nameof(Views.Cards.ChampionCardPage)}",
            CardTypes.Spell => $"{nameof(Views.Cards.SpellCardPage)}",
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
