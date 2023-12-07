public interface IAuthorRepository
{
	public AuthorDTO? GetAuthorByName(string name);
	public AuthorDTO? GetAuthorByEmail(string email);
	public void CreateNewAuthor(string name, string email);
	public void FollowAuthor(string followerName, string followeeName);
	public Task UnfollowAuthor(string followerName, string followeeName);
	public List<AuthorDTO> GetFollowing(string authorname);
	public List<AuthorDTO> GetFollowers(string authorname);

}