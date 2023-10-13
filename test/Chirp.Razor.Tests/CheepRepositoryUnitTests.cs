namespace Chirp.Razor.Tests;

using System.Diagnostics;

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

	public void Dispose() => _connection.Dispose();

	[Fact]
	public void Read_CheepNotNull()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		CheepViewModel? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void Read_CheepsDescendingOrder()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		var orderedCheeps = _cheepRepository.GetCheeps(1).OrderByDescending(cheep => cheep.Timestamp);

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
