namespace Chirp.Infrastructure.Repositories
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly ChirpDBContext _context;

		public AuthorRepository(ChirpDBContext context)
		{
			_context = context;
		}

		public void CreateNewAuthor(string name, string email)
		{
			try
			{
				_context.Authors.Add(new Author { Name = name, Email = email, Cheeps = new List<Cheep>() });
				_context.SaveChanges();
			}
			catch (Exception)
			{
				throw new Exception("Error when creating new author");
			}
		}

		public AuthorDTO? GetAuthorByEmail(string email)
		{
			return _context.Authors
				.Where(a => a.Email == email)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.SingleOrDefault();
		}

		public AuthorDTO? GetAuthorByName(string name)
		{
			return _context.Authors
				.Where(a => a.Name == name)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.SingleOrDefault();
		}
	}
}
