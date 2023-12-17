using Microsoft.EntityFrameworkCore;

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
			_context.Authors.Add(new Author { Name = name, Email = email, Cheeps = new List<Cheep>() });
			_context.SaveChanges();
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

		public List<AuthorDTO> GetFollowing(string authorname)
		{
			return _context.Authors
				.Where(a => a.Name == authorname)
				.SelectMany(a => a.Following)
				.OrderByDescending(a => a.AuthorId)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
		}

		public List<AuthorDTO> GetFollowers(string authorname)
		{
			return _context.Authors
				.Where(a => a.Name == authorname)
				.SelectMany(a => a.Followers)
				.OrderByDescending(a => a.AuthorId)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
		}

		public void FollowAuthor(string followerName, string followeeName)
		{
			var follower = _context.Authors
				.Where(a => a.Name == followerName)
				.FirstOrDefault();
			var followee = _context.Authors
				.Where(a => a.Name == followeeName)
				.FirstOrDefault();
			if (follower == null || followee == null)
			{
				throw new InvalidOperationException("Either follower or followee does not exist. No new follow could be made.");
			}
			follower.Following.Add(followee);
			followee.Followers.Add(follower);
			_context.SaveChanges();
		}

		public void UnfollowAuthor(string followerName, string followeeName)
		{
			var follower = _context.Authors
				.Include(a => a.Following)
				.Where(a => a.Name == followerName)
				.FirstOrDefault();
			var followee = _context.Authors
				.Include(a => a.Followers)
				.Where(a => a.Name == followeeName)
				.FirstOrDefault();
			if (follower == null || followee == null)
			{
				Console.WriteLine("Either follower or followee does not exist. No new follow was made");
				return;
			}
			Console.WriteLine("Unfollowed " + followee + " from " + follower);
			Console.WriteLine(follower.Following.Count);
			Console.WriteLine(followee.Followers.Count);
			followee.Followers.Remove(follower);
			follower.Following.Remove(followee);
			_context.SaveChanges();
		}

		public void DeleteAuthorByName(string name)
		{
			var authorToDelete = _context.Authors
				.Where(author => author.Name == name)
				.Include(author => author.LikedCheeps)
				.Include(author => author.Cheeps)
				.First();

			// Remove likes on authors cheeps
			_context.Cheeps.RemoveRange(authorToDelete.Cheeps);

			_context.Authors.Remove(authorToDelete);

			_context.SaveChanges();

		}
	}
}