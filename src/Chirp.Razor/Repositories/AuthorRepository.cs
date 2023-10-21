using System.Linq;
using System.Xml.Linq;

namespace Chirp.Razor.Repositories
{
    public interface IAuthorRepository
    {
        public List<Author> getAuthorByName(string name);
        public List<Author> getAuthorByEmail(string email);
        public void createNewAuthor(string name, string email);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext _context;

        public AuthorRepository(ChirpDBContext context)
        {
            _context = context;
        }

        public void createNewAuthor(string name, string email)
        {
            throw new NotImplementedException();
        }

        public List<Author> getAuthorByEmail(string email)
        {
            return _context.Authors
                .Take(_context.Authors.Count<Author>())
                .OrderByDescending(a => a.AuthorId)
                .Where(a => a.Email == email)
                .ToList();
        }

        public List<Author> getAuthorByName(string name)
        {
            return _context.Authors
                .Take(_context.Authors.Count<Author>())
                .OrderByDescending(a => a.AuthorId)
                .Where(a => a.Name == name)
                .ToList();
        }
    }
}
