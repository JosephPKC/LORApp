using System.Data;

namespace SqliteHandler;

public interface ISqlite
{
    void Create(string pTable, IEnumerable<string> pCreates);
    void Drop(string pTable);
    void Insert(string pTable, IEnumerable<string> pInsert);
    void InsertBatch(string pTable, IEnumerable<IEnumerable<string>> pInserts);
    DataTable Select(string pTable, IEnumerable<string>? pSelect, IEnumerable<KeyValuePair<string, object>>? pWhere);
}
