﻿using Chirp.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PublicModel : ChirpPage
{
	public PublicModel(
		IAuthorRepository authorRepository,
		ICheepRepository cheepRepository,
		IValidator<CreateCheepDTO> _cheepValidator) : base(authorRepository, cheepRepository, _cheepValidator
	) { }

	private async Task EnsureAuthorCreated()
	{
		// If user is not authenticated, just return
		if (User.Identity != null && !User.Identity.IsAuthenticated)
		{
			return;
		}

		string authorCookieName = "AuthorCreated";

		// If cookie exists, return
		string? authorCookie = Request.Cookies[authorCookieName];
		
		if (authorCookie != null)
		{
			return;
		}

		var authorName = User.Identity?.Name
			?? throw new Exception("User identity name is null");

        var author = AuthorRepository.GetAuthorByName(authorName);

		if (author == null)
		{
			string token = User.FindFirst("idp_access_token")?.Value
				?? throw new Exception("Github token not found");

			string email = await GithubHelper.GetUserEmailGithub(token, authorName);

			try
			{
				AuthorRepository.CreateNewAuthor(authorName, email);
			}
			catch (Exception e)
			{
				Console.WriteLine($"User creation failed: {e}");
				return;
			}
		}
		
		Response.Cookies.Append(authorCookieName, true.ToString());
	}

	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		if (User.Identity != null && User.Identity.IsAuthenticated)
		{
			EnsureAuthorCreated().Wait();
			string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
			Following = AuthorRepository.GetFollowing(name);
		}

		Cheeps = CheepRepository.GetCheeps(page);
		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount();

		return Page();
	}
}
