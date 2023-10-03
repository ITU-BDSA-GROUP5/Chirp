using Microsoft.Data.Sqlite;

namespace SQLService;

public interface IDBFacade
{
    /// <summary>
    ///     Fetches all cheeps.
    /// </summary>
    /// <returns>All cheeps from all authors.</returns>
    public List<CheepViewModel> Fetch();
    /// <summary>
    ///     Fetches cheeps from a specified author.
    /// </summary>
    /// <param name="author">The username of the author</param>
    /// <returns>All cheeps from the author.</returns>
    public List<CheepViewModel> FetchFromAuthor(string author);
}

public class DBFacade : IDBFacade
{
    //instance for singleton pattern
    private static DBFacade? instance;
    //datasource to use by default for the database
    private const string DefaultDataSource = "data/main.db";
    //datasource currently used for the database
    private string DataSource = DefaultDataSource;

    public static DBFacade GetInstance()
    {
        if (instance == null)
            instance = new DBFacade();

        instance.SetDataSource(DefaultDataSource);
        return instance;
    }
    private DBFacade() { }

    public void SetDataSource(string dataSource) { this.DataSource = dataSource; }

    public override List<CheepViewModel> Fetch()
    {
        List<CheepViewModel> cheeps = new();

        using (var connection = new SqliteConnection($"Data Source={DataSource}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT username, text, pub_date
                FROM message m
                JOIN user u ON m.author_id = u.user_id
            ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CheepViewModel cheep = new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetLong(2));

                    cheeps.Add(cheep);
                }
            }
        }

        return cheeps;
    }

    public override List<CheepViewModel> FetchFromAuthor(string author)
    {
        List<CheepViewModel> cheeps = new();

        using (var connection = new SqliteConnection($"Data Source={DataSource}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT username, text, pub_date
                FROM message m
                JOIN user u ON m.author_id = u.user_id
                WHERE username = $author
            ";
            command.Parameters.AddWithValue("$author", author);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CheepViewModel cheep = new CheepViewModel(reader.GetString(0), reader.GetString(1), reader.GetLong(2));

                    cheeps.Add(cheep);
                }
            }
        }

        return cheeps;
    }
}