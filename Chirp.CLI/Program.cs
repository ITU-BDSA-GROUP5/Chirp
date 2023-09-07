namespace Chirp.CLI;

using System.Text.RegularExpressions;

if (args.Length > 1)
    Console.WriteLine("Please, enter read to read or cheep to cheep");
else if (args[0] == "read")
    Read();
else if (args[0] == "cheep")
    Cheep(args[1]);

//Prints all past cheeps
static void Read()
{
    using (StreamReader reader = File.OpenText("chirp_cli_db.csv"))
    {
        string cheep;
        reader.ReadLine();
        while ((cheep = reader.ReadLine()) is not null)
        {
            Console.WriteLine(FormatCheep(cheep));
        }
    }
}

static string FormatCheep(string cheep)
{
    Regex cheepSegmentRegex = new Regex(@"(?<user>[a-zA-Z]+),""(?<cheep>.+)"",(?<timestamp>[0-9]+)");
    GroupCollection cheepSegments = cheepSegmentRegex.Matches(cheep)[0].Groups;
    DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cheepSegments["timestamp"].Value)).LocalDateTime;

    return $"{cheepSegments["user"].Value} @ {timestamp.ToString("G")}: {cheepSegments["cheep"].Value}";
}

static void Cheep(string message)
{
    string user = Environment.UserName;
    var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    if(!File.Exists("chirp_cli_db.csv"))
    {
        using StreamWriter sw = File.CreateText("chirp_cli_db.csv");
        sw.WriteLine($"{user},{message},{time}");
    }
    else
    {
        using StreamWriter sw = File.AppendText("chirp_cli_db.csv");
        sw.WriteLine($"{user},{message},{time}");
    }
    return;
}