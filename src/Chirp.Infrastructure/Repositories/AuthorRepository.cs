namespace Chirp.Infrastructure.Repositories
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly ChirpDBContext _context;

		public AuthorRepository(ChirpDBContext context)
		{
			_context = context;
		}

		public void CreateNewAuthor(Guid id, string name, string email)
		{
			var existing = GetAuthorByEmail(email);
			if (existing.Any())
			{
				Console.WriteLine("Author " + email + " already exists. No new author was made");
				return;
			}
			_context.Authors.Add(new Author { AuthorId = GetHumanReadableId(id), Name = name, Email = email, Cheeps = new List<Cheep>() });
			_context.SaveChanges();
		}

		private int GetHumanReadableId(Guid id) { return id.GetHashCode(); }

		public List<AuthorDTO> GetAuthorByEmail(string email)
		{
			return _context.Authors
				.Where(a => a.Email == email)
				.OrderByDescending(a => a.AuthorId)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
		}

		public List<AuthorDTO> GetAuthorByName(string name)
		{
			return _context.Authors
				.Where(a => a.Name == name)
				.OrderByDescending(a => a.AuthorId)
				.Select(a => new AuthorDTO
				{
					Name = a.Name,
					Email = a.Email
				})
				.ToList();
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
				Console.WriteLine("Either follower or followee does not exist. No new follow was made");
				return;
			}
			follower.Following.Add(followee);
			followee.Followers.Add(follower);
			_context.SaveChanges();
		}

		public void UnfollowAuthor(string followerName, string followeeName)
		{
			var follower = _context.Authors
				.Where(a => a.Name == followerName)
				.FirstOrDefault();
			var followee = _context.Authors
				.Where(a => a.Name == followeeName)
				.FirstOrDefault();
			if (follower == null || followee == null)
			{
				Console.WriteLine("Either follower or followee does not exist. No new follow was made");
				return;
			}
			followee.Followers.Remove(follower);
			follower.Following.Remove(followee);
			_context.SaveChanges();
		}
	}
}