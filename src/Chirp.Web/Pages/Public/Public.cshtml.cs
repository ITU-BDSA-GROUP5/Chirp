using System.Security.Claims;
using System.Security.Cryptography.Xml;
using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	public required List<CheepDTO> Cheeps { get; set; }

	public required List<AuthorDTO> Following { get; set; }

	[BindProperty]
	public string? CheepMessage { get; set; }

	public bool EmptyCheep { get; set; }

	public PublicModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
	{
		AuthorRepository = authorRepository;
		CheepRepository = cheepRepository;
	}

	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = CheepRepository.GetCheeps(page);
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
		Following = AuthorRepository.GetFollowing(name);
		return Page();
	}
	public IActionResult OnPost()
	{
		Console.WriteLine("OnPost called!");

		if (CheepMessage == null)
		{
			EmptyCheep = true;
			return OnGet();
		}

		string email = User.Claims.Where(a => a.Type == "emails").Select(e => e.Value).Single();
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");

		if (AuthorRepository.GetAuthorByEmail(email).SingleOrDefault() == null)
		{
			AuthorRepository.CreateNewAuthor(new Guid(), name, email);
		}

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

	public IActionResult OnPostFollow(string followeeName, string followerName)
	{
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");

		if (AuthorRepository.GetAuthorByName(name).SingleOrDefault() == null)
		{
			AuthorRepository.CreateNewAuthor(new Guid(), name, name + "@gmail.com");
		}

		AuthorRepository.FollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect("/");
	}

	public IActionResult OnPostUnfollow(string followeeName, string followerName)
	{
		AuthorRepository.UnfollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect("/");
	}
}
