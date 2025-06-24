using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class KeywordInsertBuilder : IInsertBuilder<KeywordModel>
{
    #region IInsertBuilder<KeywordModel>
    public IEnumerable<string> BuildInsertValues(KeywordModel pModel)
    {
        //  RefCode, Name, Description
        return [
            pModel.NameRef, pModel.Name, pModel.Description
        ];
    }
    #endregion
}
