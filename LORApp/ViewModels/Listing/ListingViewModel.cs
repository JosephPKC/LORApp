using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LORApp.Controllers;
using LORApp.Controllers.Listing;
using LORApp.Models.Cards;
using LORApp.Models.Listing;
using LORApp.Services.Repo;

namespace LORApp.ViewModels.Listing;

internal partial class ListingViewModel : ObservableObject
{
    private CardListModel _cardList;
    private ICardRepository _cardRepo;
    private IListingController _controller;

    #region Properties
    public ObservableCollection<CardListingModel> CardListing { get; }
    #endregion

    #region Commands
    public ICommand SelectCardCommand { get; }
    #endregion

    public ListingViewModel()
    {
        _cardRepo = CardRepositoryFactory.CreateCardRepo();
        _controller = ControllerFactory.CreateListingController();

        SelectCardCommand = new AsyncRelayCommand<CardModel>(SelectCardAsync);

        _cardList = LoadCardList();
        CardListing = [.. _cardList.CardListing];
    }

    private CardListModel LoadCardList()
    {
        //  Load up all cards from the repo
        IEnumerable<CardModel> cardList = _cardRepo.FetchAll();

        //  Turn the card models into listings
        return _controller.CreateCardList(cardList);
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
