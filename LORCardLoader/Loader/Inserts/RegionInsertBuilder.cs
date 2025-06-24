using LORCardLoader.Models;

namespace LORCardLoader.Loader.Inserts;

internal class RegionInsertBuilder : IInsertBuilder<RegionModel>
{
    #region IInsertBuilder<RegionModel>
    public IEnumerable<string> BuildInsertValues(RegionModel pModel)
    {
        //  RefCode, Name, IconPath
        return [
            pModel.NameRef, pModel.Name, pModel.IconAbsolutePath
        ];
    }
    #endregion
}
