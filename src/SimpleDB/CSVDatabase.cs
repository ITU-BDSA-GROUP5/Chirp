namespace SimpleDB;

using System.Globalization;
using CsvHelper;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
	private static CSVDatabase<T>? instance = null;
	static string CSVPath = "";
	private CSVDatabase(){}

	public string GetPath()
	{
		if (CSVPath == "")
		{
			throw new Exception("Path has not been set!");
		}

		return CSVPath;
	}

	public void SetPath(string csvPath)
	{
        if (!File.Exists(csvPath))
        {
            using (StreamWriter sw = File.CreateText(csvPath))
            {
                sw.WriteLine("Author,Message,Timestamp");
            }
        }
		CSVPath = csvPath;
	}

	public static CSVDatabase<T> GetInstance()
	{
		if (instance == null)
		{
			instance = new CSVDatabase<T>();
		}
		return instance;
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
			csv.NextRecord(); // makes a new line
		}
	}
}
