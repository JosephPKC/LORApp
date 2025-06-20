using System.Data;
using System.Data.SQLite;
using SqlKata;
using SqlKata.Compilers;

using LORApp.Services.CardRepo;
using System.Diagnostics;

namespace LORApp.Adapters.Sqlite;

internal class SqliteAdapter : IRepo, IDisposable
{
    private readonly IDbConnection _conn;

    protected bool _isDisposed = false;

    public SqliteAdapter(string pDbPath)
    {
        _conn = new SQLiteConnection($"Data Source={pDbPath};Version=3;New=False;");
        EnableWriteAheadLogging();
    }

    public SqliteAdapter(IDbConnection pConn)
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

    #region IRepo
    public DataTable Read(string pTable, IEnumerable<string>? pFields, IEnumerable<KeyValuePair<string, object>>? pFilters)
    {
        Query query = new();
        query.Select(pFields ?? ["*"]).From(pTable).Where(pFilters ?? []);
        SqliteCompiler compiler = new();
        SqlResult sqliteQuery = compiler.Compile(query);

        Trace.WriteLine(sqliteQuery.Sql);

        DataTable dt = new();

        _conn.Open();
        using IDbTransaction trans = _conn.BeginTransaction();
        try
        {
            using IDbCommand cmd = _conn.CreateCommand();
            cmd.CommandText = sqliteQuery.Sql;
            IDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
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

        return dt;
    }
    #endregion

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
