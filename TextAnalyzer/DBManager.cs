using System.Collections;
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
        ConnectionSetup(exePath + "/analyze.db");
    }

    private void ConnectionSetup(string path)
    {
        string connectionString = new SqliteConnectionStringBuilder()
        {
            // Database is in same path as program directory
            DataSource = path,
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
                    'ScanId' INTEGER PRIMARY KEY AUTOINCREMENT,
                    'ScanTime' DATETIME,
                    'SourceName' TEXT,
                    'WordCount' INT,
                    'CharCount' INT,
                    'LongestWord' TEXT,
                    CONSTRAINT 'ComposedPK' UNIQUE ('ScanTime', 'SourceName')
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

    public void SaveData(AnalyzerResult result)
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
                SELECT DISTINCT ScanId FROM Scans
                WHERE 
                    ScanTime = @ScanTime 
                        AND
                    SourceName = @SourceName
                ;
            ";

        command.Parameters.AddWithValue("@ScanTime", result.ScanTime);
        command.Parameters.AddWithValue("@SourceName", result.SourceName);
        command.Parameters.AddWithValue("@WordCount", result.TotalWordCount);
        command.Parameters.AddWithValue("@CharacterCount", result.TotalCharCount);
        command.Parameters.AddWithValue("@LongestWord", result.LongestWord);
        var reader = command.ExecuteReader();
        
        int scanId = 0;
        if (reader.Read())
        {
            scanId = reader.GetInt32(0);
        }
        if (scanId == 0)
        {
            throw new Exception("Failed to get ScanId");
        }
        
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
            command.ExecuteNonQuery();
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
            command.ExecuteNonQuery();
        }
        _dbConnection.Close();
    }

    public AnalyzerResult? GetScan(string sourceName, DateTime scanTime)
    {
        _dbConnection.Open();
        var query = _dbConnection.CreateCommand();
        query.CommandText = @"
            SELECT * FROM Scans
            WHERE (SourceName, ScanTime) = (@Source, @Time)
            LIMIT 1
        ";

        query.Parameters.AddWithValue("@Source", sourceName);
        query.Parameters.AddWithValue("@Time", scanTime);

        var reader = query.ExecuteReader();
        if (reader.Read())
        {
            return DeserializeScan(reader);
        }
        else
        {
            return null;
        }
    }

    public List<AnalyzerResult> GetWithSource(string scanName)
    {
        var scanList = new List<AnalyzerResult>();
        _dbConnection.Open();
        
        var query = _dbConnection.CreateCommand();
        query.CommandText = @"
            SELECT * FROM Scans
            WHERE SourceName = @scanName
        ";
        query.Parameters.AddWithValue("@scanName", scanName);

        var queryReader = query.ExecuteReader();
        while (queryReader.Read())
        {
            scanList.Add(DeserializeScan(queryReader));
        }
        
        _dbConnection.Close();
        return scanList;
    }

    public AnalyzerResult DeserializeScan(SqliteDataReader reader)
    {
        int scanId = reader.GetInt32(reader.GetOrdinal("ScanId"));

        var mapQuery = _dbConnection.CreateCommand();
        mapQuery.CommandText = "SELECT * FROM WordMap WHERE ScanId = @id";
            
        mapQuery.Parameters.AddWithValue("@id", scanId);
        var mapReader = mapQuery.ExecuteReader();

        var wordMap = new Dictionary<string, int>();
        while (mapReader.Read())
        {
            wordMap.Add(
                mapReader.GetString(mapReader.GetOrdinal("Word")),
                mapReader.GetInt32(mapReader.GetOrdinal("Count"))
            );
        }
        mapReader.Close();
            
        mapQuery.CommandText = "SELECT * FROM CharMap WHERE ScanId = @id";
        mapReader = mapQuery.ExecuteReader();
            
        var charMap = new Dictionary<string, int>();
        while (mapReader.Read())
        {
            charMap.Add(
                mapReader.GetString(mapReader.GetOrdinal("Character")),
                mapReader.GetInt32(mapReader.GetOrdinal("Count"))
            );
        }

        var scan = new AnalyzerResult();
        scan.ScanTime = reader.GetDateTime(reader.GetOrdinal("ScanTime"));
        scan.SourceName = reader.GetString(reader.GetOrdinal("SourceName"));
        scan.HeatmapChar = charMap;
        scan.HeatmapWord = wordMap;
        scan.LongestWord = reader.GetString(reader.GetOrdinal("LongestWord"));
        scan.TotalCharCount = reader.GetInt32(reader.GetOrdinal("CharCount"));
        scan.TotalWordCount = reader.GetInt32(reader.GetOrdinal("WordCount"));

        return scan;
    }
}