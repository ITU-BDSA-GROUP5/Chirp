using Chirp.CLI;
using SimpleDB;
using DocoptNet;

string usage = @"
Usage:
	chirp <command> [<message>]";

// This line was taken from chatgpt
var arguments = new Docopt().Apply(usage, args, version: "Chirp 1.0", exit: true);

if (arguments is null) { throw new ArgumentNullException(nameof(arguments)); }
string input = arguments["<command>"].ToString();

var userInterface = new UserInterface();

if (input == "read")
	Read();
else if (input == "cheep")
	Cheep(arguments["<message>"].ToString());

void Read()
{
	IDatabaseRepository<Cheep> db = GetDB();
	IEnumerable<Cheep> cheeps = db.Read();

	userInterface.PrintCheeps(cheeps);
}

void Cheep(string message)
{
	IDatabaseRepository<Cheep> db = GetDB();
	string author = Environment.UserName;
	var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	db.Store(new Cheep(author, message, time));
}

IDatabaseRepository<Cheep> GetDB()
{
	CSVDatabase<Cheep> db = CSVDatabase<Cheep>.GetInstance();
	db.SetPath("chirp_cli_db.csv");
	return db;
}