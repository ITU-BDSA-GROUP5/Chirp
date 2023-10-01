namespace SQLService.test;

public class SQLCheepServiceTests
{
    //Tests for saving to database


    //Tests for fetching from database


    //Integration tests

    [Fact]
    public void Fetch_AfterSaveSingleCheep_ReturnsSameCheep()
    {
        // Arrange
        ISQLCheepService service = SQLCheepService.GetInstance();
        Cheep cheep = new Cheep("Tester", 1696168349, "This is a test :)");

        // Act
        service.Save(cheep);
        IEnumerable<Cheep> cheeps = service.Fetch("Tester");

        // Assert
        Assert.True(cheeps.Contains(cheep)); //This may cause issues if AssertTrue works on the references and not the values
    }
}