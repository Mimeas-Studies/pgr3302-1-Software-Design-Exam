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
                SELECT DISTINCT 'ScanId' FROM Scans
                WHERE 
                    ScanTime = @ScanTime 
                        AND
                    SourceName = @SourceName
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
        else
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
        _dbConnection.Close();
    }

    public List<AnalyzerResult> GetAllScans(string scanName)
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
            int scanId = queryReader.GetInt32(queryReader.GetOrdinal("ScanId"));

            var mapQuery = _dbConnection.CreateCommand();
            mapQuery.CommandText = "SELECT * FROM WordMap WHERE 'ScanId' = @id";
            
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
            
            mapQuery.CommandText = "SELECT * FROM CharMap WHERE 'ScanId' = @id";
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
            scan.HeatmapChar = charMap;
            scan.HeatmapWord = wordMap;
            scan.LongestWord = queryReader.GetString(queryReader.GetOrdinal("LongestWord"));
            scan.TotalCharCount = queryReader.GetInt32(queryReader.GetOrdinal("CharCount"));
            scan.TotalWordCount = queryReader.GetInt32(queryReader.GetOrdinal("WordCount"));
            
            scanList.Add(scan);
        }
        
        _dbConnection.Close();
        return scanList;
    }
}