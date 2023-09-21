namespace Chirp.CLI.Tests;

public class UserInterfaceTests
{
    [Theory]
    [InlineData("apple","Hello world",1694437331, "apple @ 11/09/2023 15:02:11: Hello world")]
    public void FormatCheep_Cheeps_ReturnsProperString(string author, string message, long timestamp, string expectation)
    {
        // Arrange
        Cheep cheep = new Cheep(author, message, timestamp);
        string CheepText = "";

        // Act
        CheepText = UserInterface.FormatCheep(cheep);

        // Assert
        Assert.Equal(expectation, CheepText);
    }
}