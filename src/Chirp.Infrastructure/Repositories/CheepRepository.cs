using System.ComponentModel;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
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
				AuthorId = author.AuthorId,
				Author = author,
				Text = cheep.Text,
				TimeStamp = timestamp
			});
			_context.SaveChanges();
		}

		public List<CheepDTO> GetCheeps(int page)
		{
			return _context.Cheeps
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss")
				})
				.ToList();
		}

		public int GetPageAmount()
		{
			return (int)Math.Ceiling((double)_context.Cheeps
			.Select(c => c)
			.Count() / pageSize);
		}
		public int GetPageAmount(string author)
		{
			return (int)Math.Ceiling(
				(double)GetCheepsFromAuthor(author)
				.Count / pageSize
			);
		}

		// Method gets a subset of all cheeps from the author
		public List<CheepDTO> GetCheepsFromAuthor(int page, string author)
		{
			return _context.Cheeps
				.Where(c => c.Author.Name == author)
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss")
				})
				.ToList();
		}

		// Method gets all cheeps from the author
		private List<CheepDTO> GetCheepsFromAuthor(string author)
		{
			return _context.Cheeps
				.Where(c => c.Author.Name == author)
				.OrderByDescending(c => c.TimeStamp)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss")
				})
				.ToList();
		}

		public async Task LikeCheep(Guid cheepId, string author)
		{
			var authorModel = await _context.Authors
				.Where(a => a.Name == author)
				.FirstAsync();

			var cheepModel = await _context.Cheeps
				.Where(c => c.CheepId == cheepId)
				.FirstAsync();

			authorModel.LikedCheeps.Add(cheepModel);
			cheepModel.LikedBy.Add(authorModel);

			await _context.SaveChangesAsync();
		}

		public async Task UnlikeCheep(Guid cheepId, string author)
		{
			var authorModel = await _context.Authors
				.Where(a => a.Name == author)
				.Include(a => a.LikedCheeps)
				.FirstAsync();

			var cheepModel = await _context.Cheeps
				.Where(c => c.CheepId == cheepId)
				.Include(c => c.LikedBy)
				.FirstAsync();

			authorModel.LikedCheeps.Remove(cheepModel);
			cheepModel.LikedBy.Remove(authorModel);

			await _context.SaveChangesAsync();
		}

		public List<CheepDTO> GetCheepsFromAuthorAndFollowings(int page, string author, List<string> following)
		{
			following.Add(author);
			return _context.Cheeps
				.Where(c => following.Contains(c.Author.Name))
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss")
				})
				.ToList();
		}
	}
}
