public interface IAuthorRepository
{
	public AuthorDTO? GetAuthorByName(string name);
	public AuthorDTO? GetAuthorByEmail(string email);
	public void CreateNewAuthor(string name, string email);

	/// <summary>
	/// A follower is someone who follows somebody else.
	/// A followee is someone who is being followed.
	/// This method takes the names of two authors, and makes one follow the other.
	/// </summary>
	/// <param name="followerName"></param>
	/// <param name="followeeName"></param>
	/// <returns></returns>
	public Task FollowAuthor(string followerName, string followeeName);

	/// <summary>
	/// A follower is someone who follows somebody else.
	/// A followee is someone who is being followed.
	/// This method takes the names of two authors, and makes one unfollow the other.
	/// </summary>
	/// <param name="followerName"></param>
	/// <param name="followeeName"></param>
	/// <returns></returns>
	public Task UnfollowAuthor(string followerName, string followeeName);
	/// <summary>
	/// </summary>
	/// <param name="authorname"></param>
	/// <returns>The authors which the given author is following</returns>
	public List<AuthorDTO> GetFollowing(string authorname);
}