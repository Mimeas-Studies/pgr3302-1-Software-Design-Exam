using System.Reflection;
using Microsoft.Data.Sqlite;
using TextAnalyzer.Analyzer;

namespace TextAnalyzer.Db;

/// <summary>
/// An implementation of <see cref="IDbManager"/> for storing <see cref="AnalyzerResult"/> in an Sqlite database.
/// </summary>
/// <seealso cref="IDbManager"/>
public class SqliteDb : IDbManager
{
    private readonly SqliteConnection _dbConnection;

    /// <summary>
    /// A new SqliteDb instance connected to a Sqlite database at the specified path.
    /// <para>
    /// Create a new database if it doesn't already exist.
    /// </para>
    /// </summary>
    /// <param name="path">The filepath to the sqlite database.</param>
    public SqliteDb(string path)
    {
        _dbConnection = SqliteDb.ConnectionSetup(path);
        SqliteDb.TableSetup(_dbConnection);
    }

    /// <summary>
    /// A new SqliteDb instance connected to a Sqlite database located in the executables directory.
    /// <para>
    /// Create a new database if it doesn't already exist.
    /// </para>
    /// </summary>
    public SqliteDb()
    {
        Logger.Debug("Using default path for database");
        // Path points to the directory the program is in
        string exeDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath) ?? ".";
        string dbPath = exeDir + "/analyze.db";

        _dbConnection = SqliteDb.ConnectionSetup(dbPath);
        SqliteDb.TableSetup(_dbConnection);
    }

    private static SqliteConnection ConnectionSetup(string path)
    {
        string connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = path,
            Mode = SqliteOpenMode.ReadWriteCreate
        }.ToString();
        
        Logger.Info($"Connecting to '{connectionString}'");
        if (!File.Exists(path)) Logger.Warn("Database file does not exist, creating a new one");

        SqliteConnection connection = new(connectionString);

        // test connection
        try
        {
            connection.Open();
            Logger.Info("Database Connected");
            connection.Close();
        }
        catch (SqliteException err)
        {
            Logger.Error(err.ToString());
            Logger.Trace("StackTrace:\n" + err.StackTrace);
        }
        finally
        {
            connection.Close();
        }

        return connection;
    }

    private static void TableSetup(SqliteConnection connection)
    {
        Logger.Debug("Setting up Database tables");

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

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (SqliteException err)
        {
            Logger.Error("Failed to setup tables");
            Logger.Trace("Trace: \n" + err.StackTrace);
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Recreates an <see cref="AnalyzerResult"/> From the current row in the <paramref name="reader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="SqliteDataReader"/> to extract from.</param>
    /// <returns>The recreated <see cref="AnalyzerResult"/>.</returns>
    private AnalyzerResult DeserializeScan(SqliteDataReader reader)
    {
        Logger.Trace("Deserializing a scan");
        
        int scanId = reader.GetInt32(reader.GetOrdinal("ScanId"));

        Dictionary<string, int> wordHeatMap = new();
        Dictionary<string,int> charHeatMap = new();
        try
        {
            _dbConnection.Open();

            wordHeatMap = ImportWordHeatMap(scanId);
            charHeatMap = ImportCharHeatMap(scanId);
        }
        catch (SqliteException err)
        {
            Logger.Error("Failed to import Heatmaps");
            Logger.Error(err.ToString());
        }
        finally
        {
            _dbConnection.Close();
        }

        return new AnalyzerResult
        {
            ScanTime = reader.GetDateTime(reader.GetOrdinal("ScanTime")),
            SourceName = reader.GetString(reader.GetOrdinal("SourceName")),
            HeatmapChar = charHeatMap,
            HeatmapWord = wordHeatMap,
            LongestWord = reader.GetString(reader.GetOrdinal("LongestWord")),
            TotalCharCount = reader.GetInt32(reader.GetOrdinal("CharCount")),
            TotalWordCount = reader.GetInt32(reader.GetOrdinal("WordCount"))
        };
    }

    private Dictionary<string, int> ImportCharHeatMap(int scanId)
    {
        SqliteCommand mapQuery = _dbConnection.CreateCommand();
        mapQuery.CommandText = "SELECT * FROM CharMap WHERE ScanId = @id";
        SqliteDataReader mapReader = mapQuery.ExecuteReader();

        var charMap = new Dictionary<string, int>();
        while (mapReader.Read())
        {
            charMap.Add(
                mapReader.GetString(mapReader.GetOrdinal("Character")),
                mapReader.GetInt32(mapReader.GetOrdinal("Count"))
            );
        }

        return charMap;
    }

    private Dictionary<string, int> ImportWordHeatMap(int scanId)
    {
        SqliteCommand mapQuery = _dbConnection.CreateCommand();
        mapQuery.CommandText = "SELECT * FROM WordMap WHERE ScanId = @id";

        mapQuery.Parameters.AddWithValue("@id", scanId);
        SqliteDataReader mapReader = mapQuery.ExecuteReader();

        var wordMap = new Dictionary<string, int>();
        while (mapReader.Read())
        {
            wordMap.Add(
                mapReader.GetString(mapReader.GetOrdinal("Word")),
                mapReader.GetInt32(mapReader.GetOrdinal("Count"))
            );
        }
        mapReader.Close();

        return wordMap;
    }

    #region IDbManager Implementation

    public void SaveData(AnalyzerResult result)
    {
        Logger.Info($"Saving Scan for {result.SourceName}");
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
            ";

        command.Parameters.AddWithValue("@ScanTime", result.ScanTime);
        command.Parameters.AddWithValue("@SourceName", result.SourceName);
        command.Parameters.AddWithValue("@WordCount", result.TotalWordCount);
        command.Parameters.AddWithValue("@CharacterCount", result.TotalCharCount);
        command.Parameters.AddWithValue("@LongestWord", result.LongestWord);
        command.ExecuteNonQuery();

        var query = _dbConnection.CreateCommand();
        query.CommandText = @"
            SELECT ScanId FROM Scans
            WHERE (SourceName, ScanTime) = (@Source, @Time)
        ";

        query.Parameters.AddWithValue("@Source", result.SourceName);
        query.Parameters.AddWithValue("@Time", result.ScanTime);
        var reader = query.ExecuteReader();

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
        Logger.Debug($"Retrieving scan {sourceName}:{scanTime}");
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

        AnalyzerResult? result = null;
        if (reader.Read())
        {
            result = DeserializeScan(reader);
        }

        _dbConnection.Close();
        return result;
    }

    public List<AnalyzerResult> GetWithSource(string scanName)
    {
        Logger.Debug($"Retrieving all saved scans for {scanName}");
        List<AnalyzerResult> scanList = new();
        _dbConnection.Open();

        SqliteCommand query = _dbConnection.CreateCommand();
        query.CommandText = @"
            SELECT * FROM Scans
            WHERE SourceName = @scanName
        ";

        query.Parameters.AddWithValue("@scanName", scanName);

        SqliteDataReader queryReader = query.ExecuteReader();
        while (queryReader.Read())
        {
            scanList.Add(DeserializeScan(queryReader));
        }

        _dbConnection.Close();
        return scanList;
    }

    public List<AnalyzerResult> GetAll()
    {
        Logger.Debug("Retrieving all saved scans");
        _dbConnection.Open();
        SqliteCommand command = _dbConnection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Scans;
        ";

        SqliteDataReader query = command.ExecuteReader();

        var scanList = new List<AnalyzerResult>();
        while (query.Read())
        {
            scanList.Add(DeserializeScan(query));
        }

        _dbConnection.Close();
        return scanList;
    }

    #endregion
}