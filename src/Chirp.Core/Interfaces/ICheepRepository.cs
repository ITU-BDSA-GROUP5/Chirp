namespace Chirp.Core;
public interface ICheepRepository
{
	public List<CheepDTO> GetCheeps(int page);

	/// <returns>The amount of pages produced by paginating all cheeps.</returns>
	public int GetPageAmount();

	/// <returns>The amount of pages produced by paginating all cheeps from one author.</returns>
	public int GetPageAmount(string author);

	/// <returns>A specified page of cheeps from a given author. Can be used for pagination.</returns>
	public List<CheepDTO> GetCheepsFromAuthor(int page, string author);

	/// <returns>All cheeps from an author.</returns>
	public List<CheepDTO> GetCheepsFromAuthor(string author);
	public void CreateNewCheep(CreateCheepDTO createCheepDTO);

	/// <param name="cheepId">The ID of the cheep to be liked</param>
	/// <param name="author">The author who likes the cheep</param>
	public void LikeCheep(Guid cheepId, string author);

	/// <param name="cheepId">The ID of the cheep to be un-liked</param>
	/// <param name="author">The author who un-likes the cheep</param>
	public void UnlikeCheep(Guid cheepId, string author);

	/// <param name="following">List of author names which the given author follows.</param>
	/// <returns>The list of cheeps from the given author and the people they follow.</returns>
	public List<CheepDTO> GetCheepsFromAuthorAndFollowings(int page, string author, List<String> following);

	/// <summary>
	/// Like GetCheeps-method but sorted by most likes first.
	/// </summary>
	public List<CheepDTO> GetMostLikedCheeps(int page);
	public void DeleteCheep(Guid cheepId);

}