using Chirp.CLI;
using SimpleDB;

if (args.Length < 1)
    Console.WriteLine("Please, enter read to read or cheep to cheep");
else if (args[0] == "read")
    Read();
else if (args[0] == "cheep")
    Cheep(args[1]);

static void Read()
{
    IDatabaseRepository<Cheep> db = GetDB();
    IEnumerable<Cheep> cheeps = db.Read();
    foreach (Cheep cheep in cheeps) {
        Console.WriteLine(FormatCheep(cheep));
    }
}

static string FormatCheep(Cheep cheep)
{
    DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).LocalDateTime;

    return $"{cheep.Author} @ {timestamp.ToString("G")}: {cheep.Message}";
}

static void Cheep(string message)
{
    IDatabaseRepository<Cheep> db = GetDB();
    string author = Environment.UserName;
    var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    db.Store(new Cheep(author, message, time));
}

static IDatabaseRepository<Cheep> GetDB() {
    return new CSVDatabase<Cheep>("./chirp_cli_db.csv");
}