using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class ChampionCardInsertBuilder : IInsertBuilder<CardModel>
{
    #region IInsertBuilder<CardModel>
    public IEnumerable<string> BuildInsertValues(CardModel pModel)
    {
        //  CardCode, Attack, Health, LevelUpDescription, LevelUpDescriptionFormatted, LevelUpCardCode
        return [
            pModel.CardCode, pModel.Attack.ToString(), pModel.Health.ToString(),
            pModel.LevelUpDescriptionRaw, pModel.LevelUpDescription, pModel.AssociatedCardRefs.First()
        ];
    }
    #endregion
}
