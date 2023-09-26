using Chirp.CLI;
using static Chirp.CLI.UserInterface;
using System.Text.RegularExpressions;
using DocoptNet;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;


//taken from slides
string baseURL = "http://127.0.0.1:5242";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);


string usage = @"
Usage:
	chirp <command> [<message>]";

// This line was taken from chatgpt
var arguments = new Docopt().Apply(usage, args, version: "Chirp 1.0", exit: true);

if (arguments is null) { throw new ArgumentNullException(nameof(arguments)); }
string input = arguments["<command>"].ToString();

if (input == "read")
	await Read();
else if (input == "cheep")
	Cheep(arguments["<message>"].ToString());


async Task Read()
{
	try
	{
		IEnumerable<Cheep> cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
		if (cheeps != null)
		{
			PrintCheeps(cheeps);
		}
		else
		{
			Console.WriteLine("No cheeps found.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"error reading tweets: {ex.Message}");
	}
}

static void Cheep(string message)
{
	/*IDatabaseRepository<Cheep> db = GetDB();
	string author = Environment.UserName;
	var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	db.Store(new Cheep(author, message, time));*/
}