using LORApp.Controllers;
using LORApp.Controllers.Cards;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Cards;

internal partial class UnitCardViewModel : BaseCardViewModel<UnitCardModel>
{
    #region Properties
    #endregion

    public UnitCardViewModel(ICardRepository pRepo) : base(new(), CardControllerFactory.CreateController<UnitCardModel>(pRepo))
    {
        _card = new();
    }

    public UnitCardViewModel(ICardRepository pRepo, UnitCardModel pCard) : base(pCard, CardControllerFactory.CreateController<UnitCardModel>(pRepo))
    {

    }

    #region BaseCardViewModel<UnitCardModel>
    protected override void LoadCard(UnitCardModel pCard)
    {

    }

    protected override void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
    }
    #endregion

    #region Commands

    #endregion
}
