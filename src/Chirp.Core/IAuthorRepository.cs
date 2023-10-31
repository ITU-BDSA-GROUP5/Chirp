namespace Chirp.Core;
public interface IAuthorRepository
{
	public List<AuthorDTO> GetAuthorByName(string name);
	public List<AuthorDTO> GetAuthorByEmail(string email);
	public void CreateNewAuthor(Guid id, string name, string email);
}