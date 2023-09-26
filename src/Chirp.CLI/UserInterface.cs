namespace Chirp.CLI
{
	public class UserInterface
	{
		public void PrintCheeps(IEnumerable<Cheep> cheeps)
		{
			foreach (Cheep cheep in cheeps)
			{
				Console.WriteLine(FormatCheep(cheep));
			}
		}

		public string FormatCheep(Cheep cheep)
		{
			DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).LocalDateTime;

			return $"{cheep.Author} @ {timestamp.ToString("dd/MM/yyyy HH:mm:ss")}: {cheep.Message}";
		}
	}

}
