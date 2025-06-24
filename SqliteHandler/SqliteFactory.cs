namespace SqliteHandler;

public static class SqliteFactory
{
    public static ISqlite CreateSqlite(string pDbPath)
    {
        return new SqliteHandler(pDbPath);
    }
}
