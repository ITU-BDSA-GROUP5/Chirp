namespace Chirp.Razor.Tests;

public class CheepServiceUnitTests
{
	private readonly ICheepService _cheepService;

	public CheepServiceUnitTests()
	{
		_cheepService = new CheepService();
	}

	[Fact]
	public void Read_CheepNotNull()
	{
		// Arrange
		var cheeps = _cheepService.GetCheeps(1);

		// Act
		CheepViewModel? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void Read_CheepsDescendingOrder()
	{
		// Arrange
		var cheeps = _cheepService.GetCheeps(1);

		// Act
		var orderedCheeps = _cheepService.GetCheeps(1).OrderByDescending(cheep => cheep.Timestamp);

		// Assert
		Assert.Equal(cheeps, orderedCheeps);
	}
}
