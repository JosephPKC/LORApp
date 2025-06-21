using LORCardLoader.Models;

namespace LORCardLoader.Loader.InsertQueries;

internal class KeywordSqlQueryBuilder : IQueryBuilder<KeywordModel>
{
    //  RefCode (Str), Name (Str), Description (Str)

    #region IQueryBuilder<KeywordModel>
    public string BuildInsertQuery(KeywordModel pModel)
    {
        return $"('{pModel.NameRef}', '{pModel.Name}', '{pModel.Description}')";
    }
    #endregion
}