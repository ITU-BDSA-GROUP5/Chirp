using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;
using FluentValidation;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	private readonly IValidator<CreateCheepDTO> CheepValidator;
	public required List<CheepDTO> Cheeps { get; set; }
	public required List<AuthorDTO> Following { get; set; }
	public int FollowerCount { get; set; }

	[BindProperty]
	public string? CheepMessage { get; set; }
	public bool InvalidCheep { get; set; }
	public string? ErrorMessage { get; set; }

	public int PageNumber { get; set; }
	public int LastPageNumber { get; set; }
	public string? PageUrl { get; set; }


	public UserTimelineModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository, IValidator<CreateCheepDTO> _cheepValidator)
	{
		AuthorRepository = authorRepository;
		CheepRepository = cheepRepository;
		CheepValidator = _cheepValidator;
    }

	public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 1)
	{
		// if user is logged in we want to see if they are viewing their own timeline or somebody elses
		if (User.Identity != null && User.Identity.IsAuthenticated)
		{
			LoadTimelineSpecificCheeps(author, page);

			FollowerCount = AuthorRepository.GetFollowers(author).Count;
		}
		else
		{
			Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);
		}

		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount(author);
		PageUrl = HttpContext.Request.GetEncodedUrl().Split("?")[0];

		return Page();
	}

	public async Task<IActionResult> OnPost()
	{
		InvalidCheep = false;
		try
		{
			if (CheepMessage == null)
			{
				throw new Exception("Cheep is empty!");
			}

			string name = (User.Identity?.Name) ?? throw new Exception("Error in getting username");
			AuthorDTO? user = AuthorRepository.GetAuthorByName(name);

			if (user == null)
			{
				string token = User.FindFirst("idp_access_token")?.Value
					?? throw new Exception("Github token not found");


				string email = await GithubHelper.GetUserEmailGithub(token, name);

				AuthorRepository.CreateNewAuthor(name, email);
				user = AuthorRepository.GetAuthorByName(name) ?? throw new Exception("Error when getting user with name: " + name);
			}

			CreateCheepDTO cheep = new CreateCheepDTO()
			{
				Text = CheepMessage,
				Name = user.Name,
				Email = user.Email
			};

			CheepValidator.ValidateAndThrow(cheep);

			CheepRepository.CreateNewCheep(cheep);
			return Redirect("/");
		}
		catch (Exception e)
		{
			ErrorMessage = e.Message;
			InvalidCheep = true;
		}

		return OnGet(HttpContext.GetRouteValue("author")?.ToString()!);
	}

	public IActionResult OnPostFollow(string followeeName, string followerName)
	{
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");

		if (AuthorRepository.GetAuthorByName(name) == null)
		{
			AuthorRepository.CreateNewAuthor(name, name + "@gmail.com");
		}

		AuthorRepository.FollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect(PageUrl ?? "/");
	}

	public IActionResult OnPostUnfollow(string followeeName, string followerName)
	{
		AuthorRepository.UnfollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
		return Redirect(PageUrl ?? "/");
	}

	private void LoadTimelineSpecificCheeps(string author, int page)
	{
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
		Following = AuthorRepository.GetFollowing(name);
		if (name == author)
		{
			Cheeps = CheepRepository.GetCheepsFromAuthorAndFollowings(page, author, Following.Select(a => a.Name).ToList());
		}
		else
		{
			Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);
		}
	}

	public IActionResult OnPostDeleteCheep()
	{
		Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		return Redirect(PageUrl ?? "/");
	}
}
