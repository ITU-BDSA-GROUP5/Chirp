namespace SimpleDB;

using System.Globalization;
using CsvHelper;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private string CSVPath;
    public CSVDatabase(string csvPath)
    {
        this.CSVPath = csvPath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        IEnumerable<T> results = new List<T>();
        using (var reader = new StreamReader(CSVPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            results = csv.GetRecords<T>().ToList();
        }
        
        return results;
    }

    public void Store(T record)
    {
        using (var writer = new StreamWriter(CSVPath, true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
        }
    }
}
