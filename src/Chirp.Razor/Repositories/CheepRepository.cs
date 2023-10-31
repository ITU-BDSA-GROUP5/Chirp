using Chirp.Core;
namespace Chirp.Razor.Repositories
{

	public class CheepRepository : ICheepRepository
	{
		private int pageSize = 32;

		private readonly ChirpDBContext _context;

		public CheepRepository(ChirpDBContext context)
		{
			_context = context;
		}

		public void CreateNewCheep(CreateCheepDTO cheep)
		{
			var timestamp = DateTime.Now;
			Author author = new Author
			{
				AuthorId = cheep.AuthorId,
				Email = cheep.Email,
				Name = cheep.Name
			};
			_context.Cheeps.Add(new Cheep { CheepId = GetHumanReadableId(cheep.CheepGuid), AuthorId = cheep.AuthorId, Author = author, Text = cheep.Text, TimeStamp = timestamp });
			_context.SaveChanges();
		}

		private int GetHumanReadableId(Guid id) { return id.GetHashCode(); }

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
