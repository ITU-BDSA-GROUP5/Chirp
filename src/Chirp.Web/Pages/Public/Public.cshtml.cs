using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	public required List<CheepDTO> Cheeps { get; set; }


	[BindProperty]
	public string? CheepMessage { get; set; }

	public PublicModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
	{
		AuthorRepository = authorRepository;
		CheepRepository = cheepRepository;
	}

	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = CheepRepository.GetCheeps(page);

		return Page();
	}
	public IActionResult OnPost()
	{
		Console.WriteLine("OnPost called!");

		string email = User.Claims.Where(a => a.Type == "emails").Select(e => e.Value).Single();

		Console.WriteLine(email + " wrote: " + CheepMessage);

		// check if author exists
		// fetch author
		// possibly create author

		AuthorDTO? Author = AuthorRepository.GetAuthorByEmail(email).First();

		if (Author == null)
		{
			Author = CreateNewAuthor();
		}

		CreateCheepDTO cheep = new CreateCheepDTO()
		{
			CheepGuid = new Guid(),
			Text = CheepMessage,
			Name = Author.Name,
			Email = Author.Email
		};

		return Redirect("/");
	}

	private AuthorDTO CreateNewAuthor()
	{
		throw new NotImplementedException();
	}
}
