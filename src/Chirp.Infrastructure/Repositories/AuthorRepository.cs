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

		public async Task FollowAuthor(string followerName, string followeeName)
		{
			var follower = await _context.Authors
				.Where(a => a.Name == followerName)
				.FirstAsync();
			var followee = await _context.Authors
				.Where(a => a.Name == followeeName)
				.FirstAsync();
			follower.Following.Add(followee);
			followee.Followers.Add(follower);
			await _context.SaveChangesAsync();
		}

		public async Task UnfollowAuthor(string followerName, string followeeName)
		{
			var follower = await _context.Authors
				.Include(a => a.Following)
				.Where(a => a.Name == followerName)
				.FirstAsync();
			var followee = await _context.Authors
				.Include(a => a.Followers)
				.Where(a => a.Name == followeeName)
				.FirstAsync();
			followee.Followers.Remove(follower);
			follower.Following.Remove(followee);
			await _context.SaveChangesAsync();
		}
	}
}