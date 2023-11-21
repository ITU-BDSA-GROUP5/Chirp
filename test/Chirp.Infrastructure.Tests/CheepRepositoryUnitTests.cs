namespace Chirp.Infrastructure.Tests;

using Chirp.Core;
using Chirp.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryUnitTests : IAsyncLifetime
{
	private ICheepRepository? _cheepRepository;
	private ChirpDBContext? _context;
	private SqlConnection? _connection;

	private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

	// From IAsyncLifetime
	public async Task InitializeAsync()
	{	
		await _msSqlContainer.StartAsync();
		_connection = new SqlConnection(_msSqlContainer.GetConnectionString());
		await _connection.OpenAsync();
		var contextOptions = new DbContextOptionsBuilder<ChirpDBContext>()
			.UseSqlServer(_connection)
			.Options;

		// Create the schema and seed some data
		_context = new ChirpDBContext(contextOptions);

		_context.Database.EnsureCreated();
		DbInitializer.SeedDatabase(_context);

		_cheepRepository = new CheepRepository(_context);
	}

	public async Task DisposeAsync()
	{
		await _msSqlContainer.DisposeAsync();
		await _connection.DisposeAsync();
	}

	private protected void Dispose() => _connection.Dispose();

	// BEWARE this test depends on cheeps being sorted by timestamp in descending order
	[Fact]
	public void CreateNewCheep_WithValues_ExistsInDatabase()
	{
		// Arrange
		var author = new Author() { AuthorId = 13, Name = "John Doe", Email = "Johndoe@hotmail.com", Cheeps = new List<Cheep>() };
		Guid CheepId = new Guid();
		string text = "Hello world!";

		// Act
		_cheepRepository.CreateNewCheep(new CreateCheepDTO
		{
			CheepGuid = CheepId,
			AuthorId = author.AuthorId,
			Name = author.Name,
			Email = author.Email,
			Text = text
		});

		var cheeps = _cheepRepository.GetCheeps(1);
		CheepDTO cheep = cheeps.First();

		// Assert
		Assert.Equal(text, cheep.Message);
		Assert.Equal("John Doe", cheep.AuthorName);
	}

	[Fact]
	public void GetCheeps_SingleCheep_CheepNotNull()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		CheepDTO? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void GetCheeps_OnePageOfCheeps_CheepsDescendingOrder()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1).Select(c => c.ToString());

		// Act
		var orderedCheeps = _cheepRepository.GetCheeps(1).OrderByDescending(c => c.TimeStamp).Select(c => c.ToString());

		// Assert
		Assert.Equal(cheeps, orderedCheeps);
	}

	[Fact]
	public void GetCheeps_AmmountOfCheeps_EqualToPageSize()
	{
		// Arrange
		int pageSize = 32;

		// Act
		var publicTimelineCheepCount = _cheepRepository.GetCheeps(1).Count;

		// Assert
		Assert.Equal(pageSize, publicTimelineCheepCount);
	}
}
