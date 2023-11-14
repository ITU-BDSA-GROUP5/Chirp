using System.Security.Claims;
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
		string name = User.Identity.Name;

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
