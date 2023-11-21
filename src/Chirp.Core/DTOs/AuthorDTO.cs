using FluentValidation;

public class AuthorDTO
{
	public required string Name { get; set; }
	public required string Email { get; set; }
}

public class AuthorDTOValidator : AbstractValidator<AuthorDTO>
{
	public AuthorDTOValidator()
	{
		RuleFor(x => x.Email)
			.EmailAddress()
			.WithMessage("Email is invalid");
	}
}