using FluentValidation;

public class CreateCheepDTO
{
	public required string Text { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
}

public class CreateCheepDTOValidator : AbstractValidator<CreateCheepDTO>
{
	public CreateCheepDTOValidator()
	{
		RuleFor(x => x.Text)
			.Length(1, 160)
			.WithMessage("Message is of invalid size");

		RuleFor(x => x.Email)
			.EmailAddress()
			.WithMessage("Email is invalid");
	}
}