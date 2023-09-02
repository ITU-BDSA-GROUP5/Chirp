if (args.Length < 1)
    Console.WriteLine("Please, enter read to read or cheep to cheep");
else if (args[0] == "read")
    Read();
else if (args[0] == "cheep")
    Cheep(args[1]);


static void Read()
{
    return;
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