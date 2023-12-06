namespace Chirp.Infrastructure.Tests;

using Chirp.Core;
using Chirp.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;

public class CheepRepositoryUnitTests : IAsyncLifetime
{
	public required ICheepRepository _cheepRepository;
	public required ChirpDBContext _context;
	public required SqlConnection _connection;

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

	[Fact]
	public void CreateNewCheep_FromNonExistingAuthor_ThrowsException()
	{
		// Arrange
		string email = "email@domain.com";
		string name = "name";
		string cheepMessage = "Message";

		CreateCheepDTO cheep = new CreateCheepDTO()
		{
			Text = cheepMessage,
			Name = name,
			Email = email
		};

		Exception? exception = null;

		// Act
		try
		{
			_cheepRepository.CreateNewCheep(cheep);
		}
		catch (Exception e)
		{
			exception = e;
		}

		// Assert
		Assert.NotNull(exception);
	}

	// BEWARE this test depends on cheeps being sorted by timestamp in descending order
	[Fact]
	public void CreateNewCheep_WithValues_ExistsInDatabase()
	{
		// Arrange
		var author = new Author() { Name = "John Doe", Email = "Johndoe@hotmail.com", Cheeps = new List<Cheep>() };
		string text = "Hello world!";
		_context.Authors.Add(author);
		_context.SaveChanges();
		
		// Act
		_cheepRepository.CreateNewCheep(new CreateCheepDTO
		{
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

	[Fact]
	public async Task LikeCheep_AuthorStoresLikedCheep_ListWithSingleCheep()
	{
		// Arrange
		var authorLikingName = "Helge";
		
		// Get author and a cheep we assume is in the dataset
		var authorWithCheepToLike = await _context.Authors
			.Where(a => a.Name == "Helge")
			.FirstAsync();
		
		var cheepToLike = authorWithCheepToLike.Cheeps
			.First()
			.CheepId;
	
		// Act
		await _cheepRepository.LikeCheep(cheepToLike, authorLikingName);
	
		var authorLiking = await _context.Authors
			.Where(a => a.Name == authorLikingName)
			.Include(a => a.LikedCheeps)
			.FirstAsync();

		// Assert
		Assert.Single(authorLiking.LikedCheeps);
	}
}
