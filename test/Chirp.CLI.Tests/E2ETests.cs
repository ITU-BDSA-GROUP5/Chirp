namespace Chirp.CLI.Tests;
using Chirp.CLI;
using System;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;

public class E2ETests
{
	string chirpCliDirectoryPath = "./../../../../../src/Chirp.CLI";

	[Fact]
	public void TestReadCheep() // This test is adapted from Helge's presentation slides
	{
		// Arrange
		// ArrangeTestDatabase();

		// Act
		string output = "";
		using (var process = new Process())
		{
			// process.StartInfo.FileName = "/usr/bin/dotnet";
			// process.StartInfo.FileName = "/usr/local/share/dotnet/dotnet";
			// process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI read";
			process.StartInfo.FileName = "./Chirp.CLI";
			process.StartInfo.Arguments = "read";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.WorkingDirectory = chirpCliDirectoryPath;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			// Synchronously read the standard output of the spawned procd cess.
			StreamReader reader = process.StandardOutput;
			output = reader.ReadToEnd();
			process.WaitForExit();
		}
		string firstCheep = output.Split("\n")[0];

		// Assert
		Assert.StartsWith("ropf", firstCheep);
		Assert.EndsWith("BDSA students!", firstCheep);
	}

	[Fact]
	public void TestWriteCheep()
	{
		// Arrange

		// use of random number to minimize chance of identical lines
		// that could cause confusion during testing 
		var random = new Random();
		int randomNumber = random.Next(1000);
		string cheepMessage = $"this is a number: {randomNumber}";

		// Act
		using (var process = new Process())
		{
			process.StartInfo.FileName = "./Chirp.CLI";
			process.StartInfo.Arguments = $"cheep \"{cheepMessage}\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.WorkingDirectory = chirpCliDirectoryPath;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			process.WaitForExit();
		}

		// Assert

		// Read directly from database file
		IEnumerable<string> file = File.ReadLines($"{chirpCliDirectoryPath}/chirp_cli_db.csv"); // This might be inefficient for large files
		string header = file.First();
		string lastLine = file.Last();

		string actualMessage = "";
		using (var reader = new StringReader($"{header}\n{lastLine}"))
		using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			csvReader.Read();
			Cheep? cheep = csvReader.GetRecord<Cheep>();
			if (cheep != null)
			{
				actualMessage = cheep.Message;
			}
		}

		Assert.Equal(cheepMessage, actualMessage);
	}
}
