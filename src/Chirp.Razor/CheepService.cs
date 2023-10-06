public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
	public List<CheepViewModel> GetCheeps(int page);
	public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{
	private IDBFacade dbfacade;
	private int pageSize = 32;

	public CheepService()
	{
		dbfacade = DBFacade.GetInstance();
	}

	public List<CheepViewModel> GetCheeps(int page)
	{
		return dbfacade.Fetch(pageSize * (page - 1), pageSize * page);
	}

	public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
	{
		return dbfacade.FetchFromAuthor(pageSize * (page - 1), pageSize * page, author);
	}
}
