using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class UnitCardInsertBuilder : IInsertBuilder<CardModel>
{
    #region IInsertBuilder<CardModel>
    public IEnumerable<string> BuildInsertValues(CardModel pModel)
    {
        //  CardCode, Attack, Health
        return [pModel.CardCode, pModel.Attack.ToString(), pModel.Health.ToString()];
    }
    #endregion
}
