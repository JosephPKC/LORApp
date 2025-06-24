using LORCardLoader.Loader;
using LORCardLoader.Loader.Db;
using LORCardLoader.Loader.Parsing;
using SqliteHandler;

namespace LORCardLoader;

public static class Program
{
    public static void Main()
    {
        string jsonPathDir = @"..\..\..\..\DataDragon";
        string dbPath = @"..\..\..\..\CardDb.db";
        ISqlite sqlite = SqliteFactory.CreateSqlite(dbPath);
        IDbController db = new SqliteAdapter(sqlite);
        IModelParser parser = ModelParserFactory.CreateModelParser();
        CardLoader loader = new(db, parser);

        try
        {
            loader.CleanLoadIntoDb(jsonPathDir);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}