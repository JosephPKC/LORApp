using SqliteHandler;

namespace LORCardLoader.Loader.Db;

internal class SqliteAdapter(ISqlite pSqlite) : IDbController
{
    private readonly ISqlite _sqlite = pSqlite;

    #region IDbController
    public void Create(string pTable, IEnumerable<string> pFieldsToCreate)
    {
        _sqlite.Create(pTable, pFieldsToCreate);
    }

    public void Drop(string pTable)
    {
        _sqlite.Drop(pTable);
    }

    public void Insert(string pTable, IEnumerable<string> pValuesToInsert)
    {
        _sqlite.Insert(pTable, pValuesToInsert);
    }

    public void InsertBatch(string pTable, IEnumerable<IEnumerable<string>> pValuesToInsert)
    {
        _sqlite.InsertBatch(pTable, pValuesToInsert);
    }
    #endregion
}
