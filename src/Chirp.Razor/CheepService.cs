public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
	public List<CheepViewModel> GetCheeps(int page);
	public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
}

public class CheepService : ICheepService
{
	private IDBFacade dbfacade;
	private int pageSize = 37;

	public CheepService()
	{
		dbfacade = DBFacade.GetInstance();
	}

	public List<CheepViewModel> GetCheeps(int page)
	{
		return dbfacade.Fetch(pageSize * page, pageSize * (page + 1));
	}

	public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
	{
		return dbfacade.FetchFromAuthor(pageSize * page, pageSize * (page + 1), author);
	}
}
