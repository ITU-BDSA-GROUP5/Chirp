using Chirp.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests
{
	public class AuthorRepositoryIntegrationTests : IAsyncLifetime
	{
		public required IAuthorRepository _authorRepository;
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

			_authorRepository = new AuthorRepository(_context);
		}

		public async Task DisposeAsync()
		{
			await _msSqlContainer.DisposeAsync();
			await _connection.DisposeAsync();
		}

		[Fact]
		public async Task GetFollowing_AfterFollowAuthor_ReturnsFollowing()
		{
			// Arrange
			var authorFollower = _authorRepository.GetAuthorByName("Rasmus");
			var authorFollowee = _authorRepository.GetAuthorByName("Helge");

			if (authorFollower == null || authorFollowee == null)
			{
				Assert.Fail("Authors not found");
			}

			// Act
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee.Name);
			List<AuthorDTO> authorFollowers = _authorRepository.GetFollowing(authorFollower.Name);

			// Assert
			Assert.Single(authorFollowers);
		}

		[Fact]
		public async Task GetFollowing_AfterUnfollowAuthor_ReturnsEmptyList()
		{
			// Arrange
			var authorFollower = _authorRepository.GetAuthorByName("Rasmus");
			var authorFollowee = _authorRepository.GetAuthorByName("Helge");

			if (authorFollower == null || authorFollowee == null)
			{
				Assert.Fail("Authors not found");
			}

			// Act
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee.Name);
			await _authorRepository.UnfollowAuthor(authorFollower.Name, authorFollowee.Name);
			List<AuthorDTO> authorFollowers = _authorRepository.GetFollowing(authorFollower.Name);

			// Assert
			Assert.Empty(authorFollowers);
		}

		[Fact]
		public async Task GetFollowing_AfterFollowingSameAuthorTwice_ReturnsSingleFollower()
		{
			// Arrange
			var authorFollower = _authorRepository.GetAuthorByName("Rasmus");
			var authorFollowee = _authorRepository.GetAuthorByName("Helge");

			if (authorFollower == null || authorFollowee == null)
			{
				Assert.Fail("Authors not found");
			}

			// Act
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee.Name);
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee.Name);
			List<AuthorDTO> authorFollowers = _authorRepository.GetFollowing(authorFollower.Name);

			// Assert
			Assert.Single(authorFollowers);
		}

		[Fact]
		public async Task UnfollowAuthor_AuthorNotFollowing_ThrowsNoException()
		{
			// Arrange
			var authorFollower = _authorRepository.GetAuthorByName("Rasmus");
			var authorFollowee = _authorRepository.GetAuthorByName("Helge");

			if (authorFollower == null || authorFollowee == null)
			{
				Assert.Fail("Authors not found");
			}

			// Act
			try
			{
				await _authorRepository.UnfollowAuthor(authorFollower.Name, authorFollowee.Name);
			}
			catch (Exception e)
			{
				// Assert
				Assert.Fail(e.Message);
			}

			return;
		}

		[Fact]
		public async Task FollowAuthor_NonExistantAuthor_Fails()
		{
			// Arrange
			var authorNotInDatabase = "this_user_does_not_exist";

			// Assert
			await Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _authorRepository.FollowAuthor(authorNotInDatabase, authorNotInDatabase)
			);
		}

		[Fact]
		public void GetFollowing_NonExistantAuthor_ReturnsEmptyList()
		{
			// Arrange
			var authorNotInDatabase = "this_user_does_not_exist";

			// Act
			var followers = _authorRepository.GetFollowing(authorNotInDatabase);

			// Assert
			Assert.Empty(followers);
		}

		[Fact]
		public async Task GetFollowing_MultipleAuthors_ReturnsListWithMultipleItems()
		{
			// Arrange
			var authorFollower = _authorRepository.GetAuthorByName("Rasmus");
			var authorFollowee2 = _authorRepository.GetAuthorByName("Jacqualine Gilcoine");
			var authorFollowee = _authorRepository.GetAuthorByName("Helge");

			if (authorFollower == null || authorFollowee2 == null || authorFollowee == null)
			{
				Assert.Fail("Authors not found");
			}

			// Act
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee.Name);
			await _authorRepository.FollowAuthor(authorFollower.Name, authorFollowee2.Name);
			List<AuthorDTO> authorFollows = _authorRepository.GetFollowing(authorFollower.Name);

			// Assert
			Assert.Equal(2, authorFollows.Count);
		}
	}
}