using System.Data.Common;
using Dapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.Data.Sqlite;

public class DbService
{


  private DbConnection _connection;
  private ILogger<DbService> _logger;

  public DbService(ILogger<DbService> logger, IConfiguration configuration)
  {
    _logger = logger;
    _connection = _InitializeConnection(configuration);
    _InitializeDatabase();
  }

  private DbConnection _InitializeConnection(IConfiguration configuration)
  {
    _logger.LogInformation("Initializing sqlite connection");
    var dbName = configuration["DatabaseSettings:name"];
    if (dbName == null)
    {
      throw new Exception("No database name");
    }
    var connection = new SqliteConnection($"Data Source={dbName}");

    // Check if the database is initializeed
    connection.Open();
    _logger.LogInformation("Sqlite connection opened");

    return connection;
  }

  private void _InitializeDatabase()
  {
    _logger.LogInformation("Initialized db tables");

    // Create users table
    Execute(@"
      CREATE TABLE IF NOT EXISTS users (
        userId varchar NOT NULL,
        steamId varchar,
        name varchar,
        password varchar, -- Extremely secure

        PRIMARY KEY(userId)
      )
      ");


    // Create sessions table
    Execute(@"
      CREATE TABLE IF NOT EXISTS sessions(
        userId varchar NOT NULL,
        sessionId varchar NOT NULL,

        PRIMARY KEY(sessionId)
        FOREIGN KEY(userId) REFERENCES users(userId)
      )
    ");


    _logger.LogInformation("Db tables initialized");
  }

  private DbConnection _GetConnection()
  {
    return _connection;
  }

  public T QuerySingle<T>(string q, object? p = null)
  {
    var conn = _GetConnection();
    var res = conn.QuerySingle<T>(q, p);

    return res;
  }

  public IEnumerable<T> Query<T>(string q, object? p = null)
  {
    var conn = _GetConnection();
    var res = conn.Query<T>(q, p);
    return res;
  }

  public void Execute(string p, object? q = null)
  {
    var conn = _GetConnection();
    conn.Execute(p, q);
  }


}