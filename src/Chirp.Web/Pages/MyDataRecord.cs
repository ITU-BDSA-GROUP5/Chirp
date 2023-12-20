/// <param name="Followees">The authors which the user follows</param>
public record MyDataRecord(string Name, string Email, List<CheepDTO> Cheeps, List<string> Followees);