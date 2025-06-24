namespace LORCardLoader.Loader.Db;

internal interface IDbController
{
    void Create(string pTable, IEnumerable<string> pFieldsToCreate);
    void Drop(string pTable);
    void Insert(string pTable, IEnumerable<string> pValuesToInsert);
    void InsertBatch(string pTable, IEnumerable<IEnumerable<string>> pValuesToInsert);
}
