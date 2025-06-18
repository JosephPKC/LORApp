using LORApp.Controllers;
using LORApp.Models.Cards;
using LORApp.Services.Repo;

namespace LORApp.ViewModels.Cards;

internal partial class SpellCardViewModel : BaseCardViewModel<SpellCardModel>
{
    #region Properties

    #endregion

    public SpellCardViewModel(ICardRepository pRepo) : base(new(), ControllerFactory.CreateCardController<SpellCardModel>(pRepo))
    {

    }

    public SpellCardViewModel(ICardRepository pRepo, SpellCardModel pCard) : base(pCard, ControllerFactory.CreateCardController<SpellCardModel>(pRepo))
    {

    }

    #region Commands

    #endregion
}
