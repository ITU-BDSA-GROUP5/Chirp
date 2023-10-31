using Chirp.Razor.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Tests
{
	public class AuthorRepositoryUnitTests
	{
		private readonly IAuthorRepository _authorRepository;
		private readonly SqliteConnection _connection;

		public AuthorRepositoryUnitTests()
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

			_authorRepository = new AuthorRepository(context);
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
