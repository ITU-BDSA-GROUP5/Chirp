using System.ComponentModel.DataAnnotations;
using Chirp.Core;
namespace Chirp.Infrastructure.Repositories
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
			Author author = _context.Authors.Where(a => a.Email == cheep.Email).Single();

			_context.Cheeps.Add(new Cheep
				{
					CheepId = GetHumanReadableId(cheep.CheepGuid),
					AuthorId = author.AuthorId,
					Author = author,
					Text = cheep.Text,
					TimeStamp = timestamp
				});
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
				.Where(c => c.Author.Name == author)
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
	}
}
