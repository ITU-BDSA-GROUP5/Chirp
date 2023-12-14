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
		var cheeps = _cheepRepository.GetCheeps(1).Select(c => c.Id);

		// Act
		var orderedCheeps = _cheepRepository.GetCheeps(1).OrderByDescending(c => c.TimeStamp).Select(c => c.Id);

		// Assert
		Assert.Equal(orderedCheeps, cheeps);
	}

	[Fact]
	public void GetCheeps_AmountOfCheeps_EqualToPageSize()
	{
		// Arrange
		int pageSize = 32;

		// Act
		var publicTimelineCheepCount = _cheepRepository.GetCheeps(1).Count;

		// Assert
		Assert.Equal(pageSize, publicTimelineCheepCount);
	}

	[Fact]
	public void LikeCheep_AuthorStoresLikedCheep_ReturnsListWithSingleCheep()
	{
		// Arrange
		var authorLikingName = "Helge";

		// Get author and a cheep we assume is in the dataset
		var authorWithCheepToLike = _context.Authors
			.Where(a => a.Name == "Rasmus")
			.First();

		var cheepToLike = authorWithCheepToLike.Cheeps
			.First()
			.CheepId;

		// Act
		_cheepRepository.LikeCheep(cheepToLike, authorLikingName);

		var authorLiking = _context.Authors
			.Where(a => a.Name == authorLikingName)
			.Include(a => a.LikedCheeps)
			.First();

		// Assert
		Assert.Single(authorLiking.LikedCheeps);
	}

	[Fact]
	public void UnlikeCheep_AuthorDoesNotStoreCheepAsLiked_ReturnsEmptyList()
	{
		// Arrange
		var authorUnlikingName = "Helge";

		// Get author and a cheep we assume is in the dataset
		var authorWithCheepToLike = _context.Authors
			.Where(a => a.Name == "Rasmus")
			.First();

		var cheepToLike = authorWithCheepToLike.Cheeps
			.First()
			.CheepId;

		// Act
		_cheepRepository.LikeCheep(cheepToLike, authorUnlikingName);
		_cheepRepository.UnlikeCheep(cheepToLike, authorUnlikingName);

		var authorUnliking = _context.Authors
			.Where(a => a.Name == authorUnlikingName)
			.Include(a => a.LikedCheeps)
			.First();

		// Assert
		Assert.Empty(authorUnliking.LikedCheeps);
	}

	[Fact]
	public void GetMostLikedCheeps_OnePageOfCheeps_CheepsInDescingAmountOfLikes()
	{
		int page = 1;

		// Arrange
		var likingAuthor1 = "Helge";
		var likingAuthor2 = "Rasmus";

		Guid helgeCheepId = _context.Cheeps
			.Where(c => c.Text == "Hello, BDSA students!")
			.Select(c => c.CheepId)
			.First();

		Guid rasmusCheepId = _context.Cheeps
			.Where(c => c.Text == "Hej, velkommen til kurset.")
			.Select(c => c.CheepId)
			.First();

		// Like Helge's cheep
		_cheepRepository.LikeCheep(helgeCheepId, likingAuthor1);
		_cheepRepository.LikeCheep(helgeCheepId, likingAuthor2);

		// Like Rasmus' cheep
		_cheepRepository.LikeCheep(rasmusCheepId, likingAuthor1);

		List<Guid> orderedCheeps = _cheepRepository.GetMostLikedCheeps(page).OrderByDescending(c => c.Likes.Count).Select(c => c.Id).ToList();

		// Act
		List<Guid> actualCheeps = _cheepRepository.GetMostLikedCheeps(page).Select(c => c.Id).ToList();

		// Assert
		Assert.Equal(orderedCheeps, actualCheeps);
	}
}
