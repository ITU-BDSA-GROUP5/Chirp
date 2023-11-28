using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;
using FluentValidation;
using System.Security.Claims;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
	private readonly IAuthorRepository AuthorRepository;
	private readonly ICheepRepository CheepRepository;
	private readonly IValidator<CreateCheepDTO> CheepValidator;
	public required List<CheepDTO> Cheeps { get; set; }

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
		Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);

		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount(author);
		PageUrl = HttpContext.Request.GetEncodedUrl().Split("?")[0];

		return Page();
	}

	public async Task<IActionResult> OnPost()
	{
		InvalidCheep = false;
		//Console.WriteLine("OnPost called!");
		try
		{
			if (CheepMessage == null)
			{
				throw new Exception("Cheep is empty!");
			}

			string name = (User.Identity?.Name) ?? throw new Exception("Error in getting username");
			AuthorDTO? user = AuthorRepository.GetAuthorByName(name).FirstOrDefault();

			if (user == null)
			{
				string token = User.FindFirst("idp_access_token")?.Value
					?? throw new Exception("Github token not found");
                
				string email = await GithubHelper.GetUserEmailGithub(token, name);

				AuthorRepository.CreateNewAuthor(name, email);
				user = AuthorRepository.GetAuthorByName(name).First();
			}

			CreateCheepDTO cheep = new ()
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
}
