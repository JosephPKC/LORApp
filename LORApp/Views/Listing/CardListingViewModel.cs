using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LORApp.Entities.Cards;
using LORApp.Entities.Listing;
using LORApp.UseCases.Listing;
using LORApp.Views.Cards;

namespace LORApp.Views.Listing;

public partial class CardListingViewModel : ObservableObject
{
    private CardListModel _cardList;
    private readonly IListCards _listCards;

    public ICommand SelectCardCommand { get; }

    #region Properties
    private CardListingModel? _selectedCard;
    public CardListingModel? SelectedCard
    {
        get
        {
            return _selectedCard;
        }

        set
        {
            // FOR SOME REASON, this is being set as the selected item again after clearing it...
            Trace.WriteLine($"Setting SelectedCard: {value}");
            if (value is null)
            {
                Trace.WriteLine("Setting null");
                OnPropertyChanged(nameof(SelectedCard));
                _selectedCard = null;
                return;
            }

            if (value.Equals(_selectedCard))
            {
                return;
            }

            OnPropertyChanged(nameof(SelectedCard));
            _selectedCard = value;
        }
    }
    public ObservableCollection<CardListingModel> CardListing { get; }
    #endregion

    public CardListingViewModel(IListCards pListCards)
    {
        _listCards = pListCards;
        SelectCardCommand = new AsyncRelayCommand(SelectCardAsync);

        _cardList = _listCards.LoadCardList();
        CardListing = [.. _cardList.CardListing];
        Trace.WriteLine("GOT CARD LISTING: ");
        Trace.WriteLine($"{CardListing.First().CardCode}");
    }

    #region Commands
    private async Task SelectCardAsync()
    {
        Trace.WriteLine("SELECT CARD ASYNC");
        if (SelectedCard is null)
        {
            return;
        }

        string cardPagePath = SelectedCard.CardType switch
        {
            CardTypes.Champion => $"{nameof(UnitCardPage)}",
            CardTypes.Spell => $"{nameof(UnitCardPage)}",
            CardTypes.Unit => $"{nameof(UnitCardPage)}",
            _ => string.Empty
        };

        if (!string.IsNullOrWhiteSpace(cardPagePath))
        {
            Trace.WriteLine("Setting null x2");
            cardPagePath += $"?card={SelectedCard.CardCode}";
            SelectedCard = null;
            await Shell.Current.GoToAsync(cardPagePath);
        }
        else
        {
            SelectedCard = null;
        }
    }
    #endregion
}
