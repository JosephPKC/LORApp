using LORApp.Controllers;
using LORApp.Controllers.Cards;
using LORApp.Models.Cards;

namespace LORApp.ViewModels.Cards;

internal partial class SpellCardViewModel : BaseCardViewModel<SpellCardModel>
{
    #region Properties

    #endregion

    public SpellCardViewModel(ICardRepository pRepo) : base(new(), CardControllerFactory.CreateController<SpellCardModel>(pRepo))
    {

    }

    public SpellCardViewModel(ICardRepository pRepo, SpellCardModel pCard) : base(pCard, CardControllerFactory.CreateController<SpellCardModel>(pRepo))
    {

    }

    #region BaseCardViewModel<SpellCardModel>
    protected override void LoadCard(SpellCardModel pCard)
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
