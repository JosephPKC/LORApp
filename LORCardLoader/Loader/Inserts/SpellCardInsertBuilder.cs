using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class SpellCardInsertBuilder : IInsertBuilder<CardModel>
{
    #region IInsertBuilder<CardModel>
    public IEnumerable<string> BuildInsertValues(CardModel pModel)
    {
        //  CardCode, SpellSpeed
        return [pModel.CardCode, pModel.SpellSpeedRef];
    }
    #endregion
}
