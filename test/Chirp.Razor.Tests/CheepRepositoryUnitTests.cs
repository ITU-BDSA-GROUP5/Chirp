namespace Chirp.Razor.Tests;
using Chirp.Razor.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryUnitTests
{
	// private readonly ICheepRepository _cheepRepository;
	// private readonly SqliteConnection _connection;

	public CheepRepositoryUnitTests()
	{
		// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
		// at the end of the test (see Dispose below).
		// _connection = new SqliteConnection("Filename=:memory:");
		// _connection.Open();

		// // These options will be used by the context instances in this test suite, including the connection opened above.
		// var contextOptions = new DbContextOptionsBuilder<ChirpDBContext>()
		// 	.UseSqlite(_connection)
		// 	.Options;

		// // Create the schema and seed some data
		// using var context = new ChirpDBContext(contextOptions);

		// _cheepRepository = new CheepRepository(context);

	}

	// public void Dispose() => _connection.Dispose();

	[Fact]
	public void Read_CheepNotNull()
	{
		// Arrange
		using var connection = new SqliteConnection("Filename=:memory:");
		connection.Open();
		var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
		using var context = new ChirpDBContext(builder.Options);
		context.Database.EnsureCreated();
		DbInitializer.SeedDatabase(context);

		var repository = new CheepRepository(context);

		var cheeps = repository.GetCheeps(1);

		// Act
		CheepDTO? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void Read_CheepsDescendingOrder()
	{
		// Arrange
		using var connection = new SqliteConnection("Filename=:memory:");
		connection.Open();
		var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
		using var context = new ChirpDBContext(builder.Options);
		context.Database.EnsureCreated();
		DbInitializer.SeedDatabase(context);

		var repository = new CheepRepository(context);

		var cheeps = repository.GetCheeps(1);

		// Act
		var orderedCheeps = repository.GetCheeps(1).OrderByDescending(cheep => cheep.TimeStamp);

		// Assert
		Assert.Equal(cheeps, orderedCheeps);
	}
}
