namespace Chirp.Razor.Repositories
{
    public interface ICheepRepository
    {
        public List<CheepViewModel> GetCheeps(int page);
        public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
    }

    public class CheepRepository : ICheepRepository
    {
        private IDBFacade dbfacade;
        private int pageSize = 32;

        public CheepRepository()
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
}
