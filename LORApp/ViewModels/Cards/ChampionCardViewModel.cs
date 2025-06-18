using LORApp.Controllers;
using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.ViewModels.Cards;

internal partial class ChampionCardViewModel : BaseCardViewModel<ChampionCardModel>
{

    #region Properties

    #endregion

    public ChampionCardViewModel(ICardRepository pRepo) : base(new(), ControllerFactory.CreateCardController<ChampionCardModel>(pRepo))
    {

    }

    public ChampionCardViewModel(ICardRepository pRepo, ChampionCardModel pCard) : base(pCard, ControllerFactory.CreateCardController<ChampionCardModel>(pRepo))
    {

    }

    #region Commands

    #endregion
}
