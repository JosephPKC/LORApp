using LORApp.Controllers;
using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.ViewModels.Cards;

internal partial class UnitCardViewModel : BaseCardViewModel<UnitCardModel>
{
    #region Properties
    #endregion

    public UnitCardViewModel(ICardRepository pRepo) : base(new(), ControllerFactory.CreateCardController<UnitCardModel>(pRepo))
    {
        _card = new();
    }

    public UnitCardViewModel(ICardRepository pRepo, UnitCardModel pCard) : base(pCard, ControllerFactory.CreateCardController<UnitCardModel>(pRepo))
    {

    }

    #region Commands

    #endregion
}
