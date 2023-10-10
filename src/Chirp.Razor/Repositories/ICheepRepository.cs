namespace Chirp.Razor.Repositories
{
    public interface ICheepRepository
    {
        public List<CheepViewModel> GetCheeps(int page);
        public List<CheepViewModel> GetCheepsFromAuthor(int page, string author);
    }

    public class CheepRepository : ICheepRepository
    {
        //private IDBFacade dbfacade;
        private readonly ChirpDBContext _DBContext;
        private int pageSize = 32;

        public CheepRepository(ChirpDBContext dBContext)
        {
            dbfacade = DBFacade.GetInstance();
            _DBContext = dBContext;
        }

        public List<CheepViewModel> GetCheeps(int page)
        {
            var cheeps = _DBContext.Cheeps.Select(c => c);
            var list = new List<Cheep>();
            foreach (var cheep in cheeps) {
                list.Add(cheep);
            }
            return list;
            //return dbfacade.Fetch(pageSize * (page - 1), pageSize * page);
        }

        public List<CheepViewModel> GetCheepsFromAuthor(int page, string author)
        {
            return dbfacade.FetchFromAuthor(pageSize * (page - 1), pageSize * page, author);
        }
    }
}
