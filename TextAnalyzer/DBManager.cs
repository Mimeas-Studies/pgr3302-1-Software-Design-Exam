using System.Reflection;
using Microsoft.Data.Sqlite;
using TextAnalyzer.Analyzer;

namespace TextAnalyzer;

public class DBManager
{
    private SqliteConnection _dbConnection;

    public DBManager()
    {
        string? EXEPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
        if (EXEPath is null)
        {
            EXEPath = ".";
        }
        string connectionString = new SqliteConnectionStringBuilder()
        {
            // Database is in same path as program directory
            DataSource = $"{EXEPath}/analyze.db",
            Mode = SqliteOpenMode.ReadWriteCreate,
            
        }.ToString();
        _dbConnection = new SqliteConnection(connectionString);
        TableSetup(_dbConnection);

    }

    private static void TableSetup(SqliteConnection connection)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Scans {
                    'ScanId' INT,
                    'ScanTime' DATETIME,
                    'SourceName' TEXT,
                    'WordCount' INT,
                    'CharCount' INT,
                    'LongestWord' TEXT
                };

                CREATE TABLE IF NOT EXISTS WordMap {
                    'ScanId' INT,
                    'Word' TEXT,
                    'Count' INT
                };

                CREATE TABLE  IF NOT EXISTS CharMap {
                    'ScanId' INT,
                    'Character' TEXT,
                    'Count' INT
                }
            ";
        command.ExecuteNonQuery();
    }

    public void SaveData(AnalyzerResult result)
    {
        
    }
}