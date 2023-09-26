using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = GetDB();

app.MapGet("/cheeps", () => database.Read());
app.MapPost("/cheep", (Cheep cheep) => database.Store(cheep));

app.Run();

static IDatabaseRepository<Cheep> GetDB()
{
	CSVDatabase<Cheep> db = SimpleDB.CSVDatabase<Cheep>.GetInstance();
	db.SetPath("chirp_cli_db.csv");
	return db;
}
public record Cheep(string Author, string Message, long Timestamp);
