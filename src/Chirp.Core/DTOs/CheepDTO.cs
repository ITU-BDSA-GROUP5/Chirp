using FluentValidation;

public class CheepDTO
{
	public required string AuthorName { get; set; }
	public required string Message { get; set; }
	public required string TimeStamp { get; set; }
}

public class CheepDTOValidator : AbstractValidator<CheepDTO>
{
	public CheepDTOValidator()
	{
		RuleFor(x => x.Message)
			.Length(1, 160)
			.WithMessage("Message is of invalid size");

		RuleFor(x => x.TimeStamp)
			.Matches("^[0-9]{4}-[0-9]{2}-[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}$")
			.WithMessage("Timestamp of undesirable format");
	}
}