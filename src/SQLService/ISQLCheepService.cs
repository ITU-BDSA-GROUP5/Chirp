public interface ISQLCheepService {
    
    //Saves a cheep to the database
    //Arguments:
    //  - Cheep cheep, cheep to be saved
    public void SaveCheep(Cheep cheep);

    //Fetches cheeps from the database
    //Arguments:
    //  - optional string author, specifies the author whose cheeps should be fetched
    public IEnumerable<Cheep> FetchCheeps(string? author);
}

record Cheep() {string author, long timestamp, string message}