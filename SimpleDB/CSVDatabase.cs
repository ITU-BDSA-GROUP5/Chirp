namespace SimpleDB;


public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private string CSVPath;
    public CSVDatabase(string csvPath)
    {
        this.CSVPath = csvPath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(CSVPath))
        using (var csv = new CsvReader(reader))
        {
            return csv.GetRecords<T>();
        }
    }

    public void Store(T record)
    {
        
    }
}
