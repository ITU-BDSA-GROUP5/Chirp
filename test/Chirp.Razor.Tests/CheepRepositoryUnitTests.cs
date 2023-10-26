namespace Chirp.Razor.Tests;

using Chirp.Razor.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryUnitTests
{
	private readonly ICheepRepository _cheepRepository;
	private readonly SqliteConnection _connection;

	public CheepRepositoryUnitTests()
	{
		// Adapted from: https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#sqlite-in-memory

		// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
		// at the end of the test (see Dispose below).
		_connection = new SqliteConnection("Filename=:memory:");
		_connection.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		var contextOptions = new DbContextOptionsBuilder<ChirpDBContext>()
			.UseSqlite(_connection)
			.Options;

		// Create the schema and seed some data
		var context = new ChirpDBContext(contextOptions);

		context.Database.EnsureCreated();
		DbInitializer.SeedDatabase(context);

		_cheepRepository = new CheepRepository(context);
	}

	private protected void Dispose() => _connection.Dispose();

	// BEWARE this test depends on cheeps being sorted by timestamp in descending order
	[Fact]
	public void CreateNewCheep_Values()
	{
		// Arrange
		var a1 = new Author() { AuthorId = 13, Name = "John Doe", Email = "Johndoe@hotmail.com", Cheeps = new List<Cheep>() };
		Guid id = new Guid();
		string text = "Hello world!";
		
		// Act
		_cheepRepository.CreateNewCheep(id, a1.AuthorId, a1, text);
		var cheeps = _cheepRepository.GetCheeps(1);
		CheepDTO cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.Equal(text, cheep.Message);
		Assert.Equal("John Doe", cheep.AuthorName);
	}

	[Fact]
	public void Read_CheepNotNull()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		CheepDTO? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void Read_CheepsDescendingOrder()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1).Select(c => c.ToString());

		// Act
		var orderedCheeps = _cheepRepository.GetCheeps(1).OrderByDescending(c => c.TimeStamp).Select(c => c.ToString());

		// Assert
		Assert.Equal(cheeps, orderedCheeps);
	}

	[Fact]
	public void Read_CheepPageSize()
	{
		// Arrange
		int pageSize = 32;

		// Act
		var publicTimelineCheepCount = _cheepRepository.GetCheeps(1).Count;

		// Assert
		Assert.Equal(pageSize, publicTimelineCheepCount);
	}
}
