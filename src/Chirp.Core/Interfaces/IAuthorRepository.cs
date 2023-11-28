public interface IAuthorRepository
{
	public List<AuthorDTO> GetAuthorByName(string name);
	public List<AuthorDTO> GetAuthorByEmail(string email);
	public void CreateNewAuthor(string name, string email);
	public void FollowAuthor(string followerName, string followeeName);
	public void UnfollowAuthor(string followerName, string followeeName);
	public List<AuthorDTO> GetFollowing(string authorname);
}