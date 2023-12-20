using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation;

namespace Chirp.Web.Pages;

public class UserTimelineModel : ChirpPage
{
	public int FollowerCount { get; set; }

	public UserTimelineModel (IAuthorRepository authorRepository, ICheepRepository cheepRepository, IValidator<CreateCheepDTO> _cheepValidator)
		: base(authorRepository, cheepRepository, _cheepValidator) { }

	public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 1)
	{
		// if user is logged in we want to see if they are viewing their own timeline or somebody elses
		if (User.Identity != null && User.Identity.IsAuthenticated)
		{
			EnsureAuthorCreated().Wait();
			LoadTimelineSpecificCheeps(author, page);
		}
		else
		{
			Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);
			LastPageNumber = CheepRepository.GetPageAmount(author);
		}

		FollowerCount = AuthorRepository.GetFollowers(author).Count;
		PageNumber = page;

		return Page();
	}

	private void LoadTimelineSpecificCheeps(string author, int page)
	{
		string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
		Following = AuthorRepository.GetFollowing(name);
		if (name == author)
		{
			Cheeps = CheepRepository.GetCheepsFromAuthorAndFollowings(page, author, Following.Select(a => a.Name).ToList());
			LastPageNumber = CheepRepository.GetPageAmountAuthorAndFollowing(author);
		}
		else
		{
			Cheeps = CheepRepository.GetCheepsFromAuthor(page, author);
			LastPageNumber = CheepRepository.GetPageAmount(author);
		}
	}
}
