namespace Chirp.Razor.Repositories
{
    public interface IAuthorRepository
    {
        public Author getAuthorByName(string name);
        public Author getAuthorByEmail(string email);
        public Author createNewAuthor(string name, string email);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _context;

        public AuthorRepository(ChirpDBContext context)
        {
            _context = context;
        }

        public Author createNewAuthor(string name, string email)
        {
            throw new NotImplementedException();
        }

        public Author getAuthorByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Author getAuthorByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
