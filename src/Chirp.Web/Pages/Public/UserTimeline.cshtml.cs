using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	public required List<CheepDTO> Cheeps { get; set; }


	[BindProperty]
	public string? CheepMessage { get; set; }

	public UserTimelineModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
	{
		AuthorRepository = authorRepository;
		CheepRepository = cheepRepository;
	}

	public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);
		return Page();
	}

	public IActionResult OnPost()
	{
		Console.WriteLine("OnPost called!");

		if (CheepMessage == null)
		{
			return Redirect("/");
		}

		string email = User.Claims.Where(a => a.Type == "emails").Select(e => e.Value).Single();
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");

		CreateCheepDTO cheep = new CreateCheepDTO()
		{
			CheepGuid = new Guid(),
			Text = CheepMessage,
			Name = name,
			Email = email
		};

		CheepRepository.CreateNewCheep(cheep);

		return Redirect("/");
	}
}
