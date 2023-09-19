using Chirp.CLI;
using SimpleDB;
using static Chirp.CLI.UserInterface;
using System.Text.RegularExpressions;
using DocoptNet;

string usage = @"
Usage:
	chirp <command> [<message>]";

// This line was taken from chatgpt
var arguments = new Docopt().Apply(usage, args, version: "Chirp 1.0", exit: true);

if (arguments is null) { throw new ArgumentNullException(nameof(arguments)); }
string input = arguments["<command>"].ToString();

if (input == "read")
	Read();
else if (input == "cheep")
	Cheep(arguments["<message>"].ToString());

static void Read()
{
	IDatabaseRepository<Cheep> db = GetDB();
	IEnumerable<Cheep> cheeps = db.Read();

	PrintCheeps(cheeps);
}

static void Cheep(string message)
{
	IDatabaseRepository<Cheep> db = GetDB();
	string author = Environment.UserName;
	var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	db.Store(new Cheep(author, message, time));
}

static IDatabaseRepository<Cheep> GetDB()
{
	CSVDatabase<Cheep> db = SimpleDB.CSVDatabase<Cheep>.GetInstance();
	db.SetPath("chirp_cli_db.csv");
	return db;
}