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
            Guid id = new Guid();
            string name = "Alice";
            string email = "Alice@itu.dk";

            // Act
            _authorRepository.CreateNewAuthor(id, name, email);
            var authorAlice = _context.Authors.Where(c => c.Name == "Alice");

			// Assert
			Assert.Single(authorAlice);
        }


		[Fact]
		public void CreateNewAuthor_WithValues_ExistsInDatabase()
		{
			// Arrange
			Guid id = new Guid();
			string name = "John Doe";
			string email = "JohnDoe@itu.dk";

			// Act
			_authorRepository.CreateNewAuthor(id, name, email);

			var authors = _authorRepository.GetAuthorByName("John Doe");
			AuthorDTO author = authors.First();

			// Assert
			Assert.Equal(name, author.Name);
			Assert.Equal(email, author.Email);
		}

		[Fact]
		public void GetAuthorByEmail_SingleAuthor_ReturnSameEmail()
		{
			// Arrange
			string email = "Roger+Histand@hotmail.com";

			// Act
			var authors = _authorRepository.GetAuthorByEmail(email);

			// Assert
			Assert.NotNull(authors);
			Assert.Equal(email, authors.First().Email);
		}

		[Fact]
		public void GetAuthorByName_SingleAuthor_ReturnSameName()
		{
			// Arrange
			string name = "Roger Histand";

			// Act
			var authors = _authorRepository.GetAuthorByName(name);

			// Assert
			Assert.NotNull(authors);
			Assert.Equal(name, authors.First().Name);
		}
	}
}
