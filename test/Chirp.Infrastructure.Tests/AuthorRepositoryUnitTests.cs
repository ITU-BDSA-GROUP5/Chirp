using Chirp.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Tests
{
	public class AuthorRepositoryUnitTests : IAsyncLifetime
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
		public void CreateNewAuthor_SingleInDatabase()
		{
			// Arrange
			string name = "Alice";
			string email = "Alice@itu.dk";

			// Act
			_authorRepository.CreateNewAuthor(name, email);
			var authorAlice = _context.Authors.Where(c => c.Name == "Alice");

			// Assert
			Assert.Single(authorAlice);
		}


		[Fact]
		public void CreateNewAuthor_WithValues_ExistsInDatabase()
		{
			// Arrange
			string name = "John Doe";
			string email = "JohnDoe@itu.dk";

			// Act
			_authorRepository.CreateNewAuthor(name, email);

			AuthorDTO? author = _authorRepository.GetAuthorByName("John Doe");

			// Assert
			Assert.NotNull(author);
			Assert.Equal(name, author.Name);
			Assert.Equal(email, author.Email);
		}

		[Fact]
		public void GetAuthorByEmail_SingleAuthor_ReturnSameEmail()
		{
			// Arrange
			string email = "Roger+Histand@hotmail.com";

			// Act
			AuthorDTO? author = _authorRepository.GetAuthorByEmail(email);

			// Assert
			Assert.NotNull(author);
			Assert.Equal(email, author.Email);
		}

		[Fact]
		public void GetAuthorByName_SingleAuthor_ReturnSameName()
		{
			// Arrange
			string name = "Roger Histand";

			// Act
			AuthorDTO? author = _authorRepository.GetAuthorByName(name);

			// Assert
			Assert.NotNull(author);
			Assert.Equal(name, author.Name);
		}

		[Fact]
		public void Author_has_unique_name()
		{
			string name = "unique name";
			string email = "";
			_authorRepository.CreateNewAuthor(name, email);
			try
			{
				_authorRepository.CreateNewAuthor(name, email);
			}
			catch (Exception)
			{
				return; // Test passed
			}
			Assert.Fail();
		}

		[Fact]
		public void Author_has_unique_email()
		{
			string name = "";
			string email = "unique email";
			_authorRepository.CreateNewAuthor(name, email);
			try
			{
				_authorRepository.CreateNewAuthor(name, email);
			}
			catch (Exception)
			{
				return; // Test passed
			}
			Assert.Fail();
		}
	}
}
