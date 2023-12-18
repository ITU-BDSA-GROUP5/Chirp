﻿using Chirp.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class TrendingModel : ChirpPage
{
	public TrendingModel (
		IAuthorRepository authorRepository,
		ICheepRepository cheepRepository,
		IValidator<CreateCheepDTO> _cheepValidator) : base(authorRepository, cheepRepository, _cheepValidator
	) { }

	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		if (User.Identity != null && User.Identity.IsAuthenticated)
		{
			string name = (User.Identity?.Name) ?? throw new Exception("Name is null!");
			Following = AuthorRepository.GetFollowing(name);
		}

		Cheeps = CheepRepository.GetMostLikedCheeps(page);
		PageNumber = page;
		LastPageNumber = CheepRepository.GetPageAmount();

		return Page();
	}
}
