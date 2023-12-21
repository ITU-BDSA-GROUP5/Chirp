public interface IAuthorRepository
{
	public AuthorDTO? GetAuthorByName(string name);
	public AuthorDTO? GetAuthorByEmail(string email);
	public void CreateNewAuthor(string name, string email);
	public void DeleteAuthorByName(string name);

	/// <param name="followerName">Author who is going to follow another author.</param>
	/// <param name="followeeName">Author to be followed.</param>
	public void FollowAuthor(string followerName, string followeeName);

	/// <param name="followerName">Author who is going to stop following another author</param>
	/// <param name="followeeName">Author to be unfollowed</param>
	public void UnfollowAuthor(string followerName, string followeeName);

	/// <param name="authorName"></param>
	/// <returns>The list of authors which the given author follows.</returns>
	public List<AuthorDTO> GetFollowing(string authorName);

	/// <param name="authorName"></param>
	/// <returns>The list of authors who follow the given author.</returns>
	public List<AuthorDTO> GetFollowers(string authorName);
}