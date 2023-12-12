public class Author
{
	public Guid AuthorId { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public List<Cheep> Cheeps { get; set; } = new();
	public List<Cheep> LikedCheeps { get; set; } = new();
	public List<Author> Following { get; set; } = new();
	public List<Author> Followers { get; set; } = new(); 
}