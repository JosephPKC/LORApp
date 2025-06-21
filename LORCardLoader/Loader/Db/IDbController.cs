using LORCardLoader.Models;

namespace LORCardLoader.Loader.Db;

internal interface IDbController
{
    void Create(string pTable, string pCreateQuery);
    void Drop(string pTable);
    void Insert(string pTable, string pInsertQuery);
    void InsertBatch(string pTable, IEnumerable<string> pInsertQueries);
}
