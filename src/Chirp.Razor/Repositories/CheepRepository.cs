using System.Xml.Linq;

public record CheepViewModel(string Author, string Message, string Timestamp);

namespace Chirp.Razor.Repositories
{
	public interface ICheepRepository
	{
		public List<CheepViewModel> GetCheeps(int page);
		public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
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

        public List<CheepViewModel> GetCheeps(int page)
		{
			return _context.Cheeps
				.Skip(page)
				.Take(pageSize)
				.OrderByDescending(c => c.TimeStamp)
				.Select(c => new CheepViewModel(c.Author.Name, c.Text, c.TimeStamp.ToString()))
				.ToList();
		}

		public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
		{
			return _context.Cheeps
				.Skip(page)
				.Take(pageSize)
				.OrderByDescending(c => c.TimeStamp)
				.Where(c => c.Author.Name == author)
				.Select(c => new CheepViewModel(c.Author.Name, c.Text, c.TimeStamp.ToString()))
				.ToList();
		}
	}
}
