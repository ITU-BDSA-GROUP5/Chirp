using Microsoft.EntityFrameworkCore;

public record CheepViewModel(string Author, string Message, string Timestamp);

namespace Chirp.Razor.Repositories
{
	public interface ICheepRepository
	{
		public List<CheepViewModel> GetCheeps(int page);
		public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
	}

	public class CheepRepository : ICheepRepository
	{
		private int pageSize = 32;
		protected DbSet<Author> Authors;
		protected DbSet<Cheep> Cheeps;
		// private readonly ChirpDBContext _dbContext;

		public CheepRepository(ChirpDBContext context)
		{
			Authors = context.Set<Author>();
			Cheeps = context.Set<Cheep>();
		}

		public List<CheepViewModel> GetCheeps(int page)
		{
			return Cheeps
				.Skip(page)
				.Take(pageSize)
				.Select(c => new CheepViewModel(c.Author.Name, c.Text, c.TimeStamp.ToString()))
				.ToList();
		}


		public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
		{
			return Cheeps
				.Skip(page)
				.Take(pageSize)
				.Where(c => c.Author.Name == author)
				.Select(c => new CheepViewModel(c.Author.Name, c.Text, c.TimeStamp.ToString()))
				.ToList();
		}
	}
}
