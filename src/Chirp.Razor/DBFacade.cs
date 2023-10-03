using Microsoft.Data.Sqlite;

public interface IDBFacade
{
	/// <summary>
	///     Fetches all cheeps in range, sorted by time in a descending order.
	/// </summary>
	/// <param name="startIndex">Index of first cheep in range.</param>
	/// <param name="endIndex">First index out of range.</param>
	/// <returns>All cheeps from all authors.</returns>
	public List<CheepViewModel> Fetch(int startIndex, int endIndex);
	/// <summary>
	///     Fetches cheeps from a specified author.
	/// </summary>
	/// <param name="startIndex">Index of first cheep in range.</param>
	/// <param name="endIndex">First index out of range.</param>
	/// <param name="author">The username of the author</param>
	/// <returns>All cheeps from the author.</returns>
	public List<CheepViewModel> FetchFromAuthor(int startIndex, int endIndex, string author);
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

	public List<CheepViewModel> Fetch(int startIndex, int endIndex)
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
                ORDER BY m.pub_date DESC
                LIMIT $limit OFFSET $offset
            ";
			command.Parameters.AddWithValue("$limit", endIndex - startIndex);
			command.Parameters.AddWithValue("$offset", startIndex);


			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					CheepViewModel cheep = new CheepViewModel(
						reader.GetString(0),
						reader.GetString(1),
						UnixTimeStampToDateTimeString(reader.GetInt64(2))
					);

					cheeps.Add(cheep);
				}
			}
		}

		return cheeps;
	}

	public List<CheepViewModel> FetchFromAuthor(int startIndex, int endIndex, string author)
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
                ORDER BY m.pub_date DESC
                LIMIT $limit OFFSET $offset
            ";
			command.Parameters.AddWithValue("$limit", endIndex - startIndex);
			command.Parameters.AddWithValue("$offset", startIndex);

			command.Parameters.AddWithValue("$author", author);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					CheepViewModel cheep = new CheepViewModel(
						reader.GetString(0),
						reader.GetString(1),
						UnixTimeStampToDateTimeString(reader.GetInt64(2))
					);

					cheeps.Add(cheep);
				}
			}
		}

		return cheeps;
	}

	private static string UnixTimeStampToDateTimeString(long unixTimeStamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTime = dateTime.AddSeconds(unixTimeStamp);
		return dateTime.ToString("dd/MM/yy H:mm:ss");
	}
}