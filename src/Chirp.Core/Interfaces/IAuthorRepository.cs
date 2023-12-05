public interface IAuthorRepository
{
	public List<AuthorDTO> GetAuthorByName(string name);
	public List<AuthorDTO> GetAuthorByEmail(string email);
	public void CreateNewAuthor(string name, string email);
	public void DeleteAuthorByName(string name);
}