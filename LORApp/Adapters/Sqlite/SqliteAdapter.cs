using System.Data;

using LORApp.Services.CardRepo;
using SqliteHandler;

namespace LORApp.Adapters.Sqlite;

internal class SqliteAdapter(ISqlite pSqlite) : IRepo
{
    private readonly ISqlite _sqlite = pSqlite;

    #region IRepo
    public DataTable Read(string pTable, IEnumerable<string>? pFields, IEnumerable<KeyValuePair<string, object>>? pFilters)
    {
        return _sqlite.Select(pTable, pFields, pFilters);
    }
    #endregion
}
