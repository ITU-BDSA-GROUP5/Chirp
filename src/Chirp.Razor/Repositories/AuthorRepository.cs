namespace Chirp.Razor.Repositories
{
    public interface IAuthorRepository
    {
        public Author getAuthorByName(string name);
        public Author getAuthorByEmail(string email);
        public Author createNewAuthor(string name, string email);
    }

    public class AuthorRepository
    {
    }
}
