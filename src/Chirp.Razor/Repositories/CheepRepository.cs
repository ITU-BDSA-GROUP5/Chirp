namespace Chirp.Razor.Repositories
{
	public interface ICheepRepository
	{
		public List<CheepDTO> GetCheeps(int page);
		public List<CheepDTO> GetCheepsFromAuthor(int page, string author);
		public void createNewCheep(Guid id, int authorid, Author author, string text);
	}

	public class CheepRepository : ICheepRepository
	{
		private int pageSize = 32;

		private readonly ChirpDBContext _context;

		public CheepRepository(ChirpDBContext context)
		{
			_context = context;
		}

		public void createNewCheep(Guid id, int authorid, Author author, string text)
		{
			var timestamp = DateTime.Now;
			_context.Cheeps.Add(new Cheep { CheepId = getHumanReadableId(id), AuthorId = authorid, Author = author, Text = text, TimeStamp = timestamp });
			_context.SaveChanges();
		}

		public int getHumanReadableId(Guid id) { return id.GetHashCode(); }

		public List<CheepDTO> GetCheeps(int page)
		{
			return _context.Cheeps
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString()
				})
				.ToList();
		}

		public List<CheepDTO> GetCheepsFromAuthor(int page, string author)
		{
			return _context.Cheeps
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Where(c => c.Author.Name == author)
				.Select(c => new CheepDTO
				{
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString()
				})
				.ToList();
		}
	}
}
