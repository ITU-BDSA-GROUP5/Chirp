/// <summary>
/// CheepDTO is intended for sending the information of a Cheep throughout the program
/// </summary>
public class CheepDTO
{
	public required Guid Id { get; set; }
	public required string AuthorName { get; set; }
	public required string Message { get; set; }
	public required string TimeStamp { get; set; }
	public required List<string> Likes { get; set; }
}