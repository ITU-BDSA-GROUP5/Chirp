namespace Chirp.Razor.Repositories
{
	public interface IAuthorRepository
	{
		public List<AuthorDTO> GetAuthorByName(string name);
		public List<AuthorDTO> GetAuthorByEmail(string email);
		public void CreateNewAuthor(Guid id, string name, string email);
		public int GetHumanReadableId(Guid id);
	}

	public class AuthorRepository : IAuthorRepository
	{
		private readonly ChirpDBContext _context;

		public AuthorRepository(ChirpDBContext context)
		{
			_context = context;
		}

		public void CreateNewAuthor(Guid id, string name, string email)
		{
			var existing = GetAuthorByEmail(email);
			if (existing.Any())
			{
				//Console.WriteLine("Author " + email + " already exists");
				return;
			}
			_context.Authors.Add(new Author { AuthorId = GetHumanReadableId(id), Name = name, Email = email, Cheeps = new List<Cheep>() });
			_context.SaveChanges();
		}

		public int GetHumanReadableId(Guid id) { return id.GetHashCode(); }

		public List<AuthorDTO> GetAuthorByEmail(string email)
		{
			return _context.Authors
				.OrderByDescending(a => a.AuthorId)
				.Where(a => a.Email == email)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
		}

		public List<AuthorDTO> GetAuthorByName(string name)
		{
			return _context.Authors
				.OrderByDescending(a => a.AuthorId)
				.Where(a => a.Name == name)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
		}
	}
}
