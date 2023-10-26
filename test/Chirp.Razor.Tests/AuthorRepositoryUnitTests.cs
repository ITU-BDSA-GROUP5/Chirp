using Chirp.Razor.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Razor.Tests
{
    internal class AuthorRepositoryUnitTests
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
    }
}
