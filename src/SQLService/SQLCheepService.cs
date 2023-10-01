using System.ComponentModel;

namespace SQLService;

public class SQLCheepService : ISQLCheepService
{
    //instance for singleton pattern
    private static SQLCheepService? instance;
    //datasource to use by default for the database
    private const string DefaultDataSource = "main.db";
    //datasource currently used for the database
    private string DataSource = DefaultDataSource;

    public static SQLCheepService GetInstance()
    {
        if (instance == null)
            instance = new SQLCheepService();

        instance.SetDataSource(DefaultDataSource);
        return instance;
    }
    private SQLCheepService() { }

    public void SetDataSource(string dataSource) { this.DataSource = dataSource; }

    public void Save(Cheep cheep)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> Fetch(string? author)
    {
        throw new NotImplementedException();
    }
}