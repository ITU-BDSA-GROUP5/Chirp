namespace Chirp.Razor.Tests;
using Chirp.Razor.Repositories;

public class CheepRepositoryUnitTests
{
	private readonly ICheepRepository _cheepRepository;

	public CheepRepositoryUnitTests()
	{
		_cheepRepository = new CheepRepository();
	}

	[Fact]
	public void Read_CheepNotNull()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		CheepViewModel? cheep = cheeps.FirstOrDefault();

		// Assert
		Assert.NotNull(cheep);
	}

	[Fact]
	public void Read_CheepsDescendingOrder()
	{
		// Arrange
		var cheeps = _cheepRepository.GetCheeps(1);

		// Act
		var orderedCheeps = _cheepRepository.GetCheeps(1).OrderByDescending(cheep => cheep.Timestamp);

		// Assert
		Assert.Equal(cheeps, orderedCheeps);
	}
}
