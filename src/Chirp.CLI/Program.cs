namespace Chirp.CLI;

using Chirp.CLI;
using SimpleDB;
using System.Text.RegularExpressions;
using DocoptNet;

public class Program {

	public static void Main(string[] args) {
		CSVDatabase<Cheep> db = SimpleDB.CSVDatabase<Cheep>.GetInstance();
		db.SetPath("chirp_cli_db.csv");
		CLI cli = new CLI(db);

		string usage = @"
		Usage:
			chirp <command> [<message>]";

		// This line was taken from chatgpt
		var arguments = new Docopt().Apply(usage, args, version: "Chirp 1.0", exit: true);

		if (arguments is null) { throw new ArgumentNullException(nameof(arguments)); }
		string input = arguments["<command>"].ToString();

		if (input == "read")
			cli.Read();
		else if (input == "cheep")
			cli.Cheep(arguments["<message>"].ToString());
		}
}