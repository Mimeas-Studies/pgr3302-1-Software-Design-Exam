using System.Reflection;
using Microsoft.Data.Sqlite;
using TextAnalyzer.Analyzer;

namespace TextAnalyzer;

public class DbManager
{
    private SqliteConnection _dbConnection;

    public DbManager(string path)
    {
        ConnectionSetup(path);
    }
    public DbManager()
    {
        string? exePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
        if (exePath is null)
        {
            exePath = ".";
        }
        ConnectionSetup(exePath);
    }

    private void ConnectionSetup(string path)
    {
        string connectionString = new SqliteConnectionStringBuilder()
        {
            // Database is in same path as program directory
            DataSource = $"{path}/analyze.db",
            Mode = SqliteOpenMode.ReadWriteCreate,
        }.ToString();

        _dbConnection = new SqliteConnection(connectionString);
        DbManager.TableSetup(_dbConnection);
    }

    private static void TableSetup(SqliteConnection connection)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Scans (
                    'ScanId' INTEGER PRIMARY KEY autoincrement,
                    'ScanTime' DATETIME,
                    'SourceName' TEXT,
                    'WordCount' INT,
                    'CharCount' INT,
                    'LongestWord' TEXT,
                    CONSTRAINT 'UniqueTimeName' UNIQUE ('ScanTime', 'SourceName')
                );

                CREATE TABLE IF NOT EXISTS WordMap (
                    'ScanId' INT REFERENCES 'Scans' ('ScanId'),
                    'Word' TEXT,
                    'Count' INT
                );

                CREATE TABLE  IF NOT EXISTS CharMap (
                    'ScanId' INT REFERENCES 'Scans' ('ScanId'),
                    'Character' TEXT,
                    'Count' INT
                )
            ";
        command.ExecuteNonQuery();
    }

    public void SaveData(string sourceName, AnalyzerResult result)
    {
        _dbConnection.Open();
        SqliteCommand command = _dbConnection.CreateCommand();
        command.CommandText = 
            @"
                INSERT INTO Scans 
                (ScanTime, SourceName, WordCount, CharCount, LongestWord)
                VALUES (
                        @ScanTime,
                        @SourceName,
                        @WordCount,
                        @CharacterCount,
                        @LongestWord
                );
                SELECT DISTINCT 'ScanId' FROM Scans 
                    WHERE 
                        'ScanTime' IS @ScanTime
                            AND
                        'SourceName' IS @SourceName
                ;
            ";

        command.Parameters.AddWithValue("@ScanTime", DateTime.Now);
        command.Parameters.AddWithValue("@SourceName", sourceName);
        command.Parameters.AddWithValue("@WordCount", result.TotalWordCount);
        command.Parameters.AddWithValue("@CharacterCount", result.TotalCharCount);
        command.Parameters.AddWithValue("@LongestWord", result.LongestWord);
        var reader = command.ExecuteReader();
        var scanId = reader.GetInt32(reader.GetOrdinal("ScanId"));

        foreach (var pair in result.HeatmapChar)
        {
            command = _dbConnection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO CharMap
                    ('ScanId', 'Character', 'Count')
                    VALUES (
                        @ScanId,
                        @Character,
                        @Count
                    );
                ";

            command.Parameters.AddWithValue("@ScanId", scanId);
            command.Parameters.AddWithValue("@Character", pair.Key);
            command.Parameters.AddWithValue("@Count", pair.Value);
        }
        foreach (var pair in result.HeatmapWord)
        {
            command = _dbConnection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO WordMap
                    ('ScanId', 'Word', 'Count')
                    VALUES (
                        @ScanId,
                        @Word,
                        @Count
                    );
                ";

            command.Parameters.AddWithValue("@ScanId", scanId);
            command.Parameters.AddWithValue("@Word", pair.Key);
            command.Parameters.AddWithValue("@Count", pair.Value);
        }
    }
}