/// <summary>
/// This represents the Cheeps in the Chirp Application, and are used by EF Core to generate migrations for the database.
/// </summary>
public class Cheep
{
	public Guid CheepId { get; set; }
	public required Author Author { get; set; }
	public Guid AuthorId { get; set; }
	public required string Text { get; set; }
	public DateTime TimeStamp { get; set; }
	public List<Author> LikedBy { get; set; } = new();
}