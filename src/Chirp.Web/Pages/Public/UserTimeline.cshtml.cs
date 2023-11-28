using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	public required List<CheepDTO> Cheeps { get; set; }

	public int PageNumber { get; set; }
	public int LastPageNumber { get; set; }
	public string? PageUrl { get; set; }

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

		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount(author);
		PageUrl = HttpContext.Request.GetEncodedUrl().Split("?")[0];

		return Page();
	}

	public async Task<IActionResult> OnPost()
	{
		Console.WriteLine("OnPost called!");

		if (CheepMessage == null)
		{
			return Redirect("/");
		}

		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
		string email = "";
		
		string? token = User.Claims.Where(a => a.Type == "idp_access_token").Select(e => e.Value).SingleOrDefault();

		if (token != null)
		{
			email = await GithubHelper.GetUserEmailGithub(token, name);
		}

		if (AuthorRepository.GetAuthorByEmail(email).SingleOrDefault() == null)
		{
			AuthorRepository.CreateNewAuthor(name, email);
		}

		CreateCheepDTO cheep = new CreateCheepDTO()
		{
			Text = CheepMessage,
			Name = name,
			Email = email
		};

		CheepRepository.CreateNewCheep(cheep);

		return Redirect("/");
	}
}
