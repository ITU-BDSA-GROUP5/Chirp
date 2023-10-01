namespace SQLService

public interface ISQLCheepService
{
    
    //Saves a cheep to the database
    //Arguments:
    //  - Cheep cheep, cheep to be saved
    public void Save(Cheep cheep);

    //Fetches cheeps from the database
    //Arguments:
    //  - optional string author, specifies the author whose cheeps should be fetched
    public IEnumerable<Cheep> Fetch(string? author);
}

public record Cheep() {string author, long timestamp, string message}