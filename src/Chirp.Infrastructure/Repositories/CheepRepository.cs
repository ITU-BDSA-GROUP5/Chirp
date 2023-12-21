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

		public void DeleteCheep(Guid cheepId)
		{
			var cheep = _context.Cheeps
				.Where(c => c.CheepId == cheepId)
				.First();

			_context.Cheeps.Remove(cheep);
			_context.SaveChanges();
		}

		public List<CheepDTO> GetCheeps(int page)
		{
			return _context.Cheeps
				.OrderByDescending(c => c.TimeStamp)
				.Include(c => c.LikedBy)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss"),
					Likes = c.LikedBy.Select(a => a.Name).ToList()
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
		public int GetPageAmountAuthorAndFollowing(string author)
		{
			// Get the amount of cheeps from all the authors which the given author follows
			var followeeCheepCount = _context.Authors
				.Where(a => a.Name == author)
				.SelectMany(a => a.Following)
				.SelectMany(a => a.Cheeps)
				.Select(c => c)
				.Count();

			return (int)Math.Ceiling(
				(double)(followeeCheepCount + GetCheepsFromAuthor(author)
				.Count) / pageSize);
		}

		// Method gets a subset of all cheeps from the author
		public List<CheepDTO> GetCheepsFromAuthor(int page, string author)
		{
			return _context.Cheeps
				.Where(c => c.Author.Name == author)
				.Include(c => c.LikedBy)
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss"),
					Likes = c.LikedBy.Select(a => a.Name).ToList()
				})
				.ToList();
		}

		// Method gets all cheeps from the author
		public List<CheepDTO> GetCheepsFromAuthor(string author)
		{
			return _context.Cheeps
				.Where(c => c.Author.Name == author)
				.Include(c => c.LikedBy)
				.OrderByDescending(c => c.TimeStamp)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss"),
					Likes = c.LikedBy.Select(a => a.Name).ToList()
				})
				.ToList();
		}

		public void LikeCheep(Guid cheepId, string author)
		{
			var authorModel = _context.Authors
				.Where(a => a.Name == author)
				.First();

			var cheepModel = _context.Cheeps
				.Where(c => c.CheepId == cheepId)
				.First();

			authorModel.LikedCheeps.Add(cheepModel);
			cheepModel.LikedBy.Add(authorModel);

			_context.SaveChanges();
		}

		public void UnlikeCheep(Guid cheepId, string author)
		{
			var authorModel = _context.Authors
				.Where(a => a.Name == author)
				.Include(a => a.LikedCheeps)
				.First();

			var cheepModel = _context.Cheeps
				.Where(c => c.CheepId == cheepId)
				.Include(c => c.LikedBy)
				.First();

			authorModel.LikedCheeps.Remove(cheepModel);
			cheepModel.LikedBy.Remove(authorModel);

			_context.SaveChanges();
		}

		public List<CheepDTO> GetCheepsFromAuthorAndFollowings(int page, string author, List<string> following)
		{
			following.Add(author);
			return _context.Cheeps
				.Where(c => following.Contains(c.Author.Name))
				.Include(c => c.LikedBy)
				.OrderByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss"),
					Likes = c.LikedBy.Select(a => a.Name).ToList()
				})
				.ToList();
		}

		public List<CheepDTO> GetMostLikedCheeps(int page)
		{
			return _context.Cheeps
				.Include(c => c.LikedBy)
				.OrderByDescending(c => c.LikedBy.Count)
				.ThenByDescending(c => c.TimeStamp)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CheepDTO
				{
					Id = c.CheepId,
					AuthorName = c.Author.Name,
					Message = c.Text,
					TimeStamp = c.TimeStamp.ToString("yyyy-MM-dd H:mm:ss"),
					Likes = c.LikedBy.Select(a => a.Name).ToList()
				})
				.ToList();
		}
	}
}
