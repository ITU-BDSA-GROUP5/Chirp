﻿using System.Text.RegularExpressions;

if (args.Length > 1)
    Console.WriteLine("Please, enter read to read or cheep to cheep");
else if (args[0] == "read")
    Read();
else if (args[0] == "cheep")
    Cheep();

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

static string FormatCheep(string cheep){
    Regex cheepSegmentRegex = new Regex(@"(?<user>[a-zA-Z]+),""(?<cheep>.+)"",(?<timestamp>[0-9]+)");
    GroupCollection cheepSegments = cheepSegmentRegex.Matches(cheep)[0].Groups;
    DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cheepSegments["timestamp"].Value));
    
    return $"{cheepSegments["user"].Value} @ {timestamp}: {cheepSegments["cheep"].Value}";
}

static void Cheep()
{
    return;
}