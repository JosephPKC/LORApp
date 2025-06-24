using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

using SqlKata;
using SqlKata.Compilers;

namespace SqliteHandler;

internal class SqliteHandler : ISqlite, IDisposable
{
    private readonly IDbConnection _conn;

    protected bool _isDisposed = false;

    public SqliteHandler(string pDbPath)
    {
        _conn = new SQLiteConnection($"Data Source={pDbPath};Version=3;New=False;");
        EnableWriteAheadLogging();
    }

    public SqliteHandler(IDbConnection pConn)
    {
        _conn = pConn;
        EnableWriteAheadLogging();
    }

    private void EnableWriteAheadLogging()
    {
        try
        {
            _conn.Open();
            IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                PRAGMA journal_mode = 'wal'
            ";

            cmd.ExecuteNonQuery();
        }
        finally
        {
            _conn.Close();
        }
    }

    #region ISqlite
    public void Create(string pTable, IEnumerable<string> pCreates)
    {
        string query = $"CREATE TABLE {pTable} ({string.Join(",", pCreates)});";
        Trace.WriteLine($"EXECUTE: {query}");

        int res = DoTransaction(query);
        Trace.Write($"Modified {res} rows.");
    }

    public void Drop(string pTable)
    {
        string query = $"DROP TABLE {pTable};";
        Trace.WriteLine($"EXECUTE: {query}");

        int res = DoTransaction(query);
        Trace.Write($"Modified {res} rows.");
    }

    public void Insert(string pTable, IEnumerable<string> pInsert)
    {
        IEnumerable<string> quotedInsert = pInsert.Select(x => CleanStringForSqlite(x));
        string query = $"INSERT INTO {pTable} VALUES ({string.Join(",", quotedInsert)});";
        Trace.WriteLine($"EXECUTE: {query}");

        int res = DoTransaction(query);
        Trace.Write($"Modified {res} rows.");
    }

    public void InsertBatch(string pTable, IEnumerable<IEnumerable<string>> pInserts)
    {
        List<string> completeInserts = [];
        foreach (IEnumerable<string> insert in pInserts)
        {
            IEnumerable<string> quotedInsert = insert.Select(x => CleanStringForSqlite(x));
            completeInserts.Add($"({string.Join(',', quotedInsert)})");
        }
        string query = $"INSERT INTO {pTable} VALUES {string.Join(",", completeInserts)}";
        Trace.WriteLine($"EXECUTE: {query}");

        int res = DoTransaction(query);
        Trace.Write($"Modified {res} rows.");
    }

    public DataTable Select(string pTable, IEnumerable<string>? pSelect, IEnumerable<KeyValuePair<string, object>>? pWhere)
    {
        Query query = new();
        query.Select(pSelect ?? ["*"]).From(pTable).Where(pWhere ?? []);
        SqliteCompiler compiler = new();
        SqlResult sqliteQuery = compiler.Compile(query);

        Trace.WriteLine($"SELECT: {query}");

        DataTable dt = DoQueryTransaction(sqliteQuery.Sql);
        Trace.WriteLine($"Received {dt.Rows.Count} rows.");

        return dt;
    }
    #endregion

    private string CleanStringForSqlite(string pStr)
    {
        return $"'{pStr.Replace("'", "''")}'";
    }

    private int DoTransaction(string pQuery)
    {
        int res = 0;
        _conn.Open();
        using IDbTransaction trans = _conn.BeginTransaction();
        try
        {
            using IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = pQuery;
            res = cmd.ExecuteNonQuery();
            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
        finally
        {
            _conn.Close();
        }

        return res;
    }

    private DataTable DoQueryTransaction(string pQuery)
    {
        DataTable dt = new();
        _conn.Open();
        try
        {
            using IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = pQuery;
            IDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
        }
        catch
        {
            throw;
        }
        finally
        {
            _conn.Close();
        }

        return dt;
    }

    #region IDisposable
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool pIsDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (pIsDisposing)
        {
            _conn.Dispose();
        }

        _isDisposed = true;
    }
    #endregion
}
