namespace Chirp.CLI.Tests;
using System;
using System.Diagnostics;
using System.ComponentModel;

public class E2ETests
{
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
			process.StartInfo.WorkingDirectory = "./../../../../../src/Chirp.CLI";
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			// Synchronously read the standard output of the spawned procd cess.
			StreamReader reader = process.StandardOutput;
			output = reader.ReadToEnd();
			process.WaitForExit();
		}
		string fstCheep = output.Split("\n")[0];

		// Assert
		Assert.StartsWith("ropf", fstCheep);
		Assert.EndsWith("BDSA students!", fstCheep);
	}
}
