using LORApp.Controllers;
using LORApp.Controllers.Cards;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Cards;

internal partial class ChampionCardViewModel : BaseCardViewModel<ChampionCardModel>
{

    #region Properties

    #endregion

    public ChampionCardViewModel(ICardRepository pRepo) : base(new(), CardControllerFactory.CreateController<ChampionCardModel>(pRepo))
    {

    }

    public ChampionCardViewModel(ICardRepository pRepo, ChampionCardModel pCard) : base(pCard, CardControllerFactory.CreateController<ChampionCardModel>(pRepo))
    {

    }

    #region BaseCardViewModel<ChampionCardModel>
    protected override void LoadCard(ChampionCardModel pCard)
    {
        _card = pCard;
    }

    protected override void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
    }
    #endregion

    #region Commands

    #endregion
}
