﻿using Chirp.Razor.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
	private readonly ICheepRepository _repository;
	public List<CheepViewModel> Cheeps { get; set; }

	public UserTimelineModel(ICheepRepository repository)
	{
		_repository = repository;
	}

	public ActionResult OnGet(string author, [FromQuery(Name = "page")] int page = 0)
	{
		Cheeps = _repository.GetCheepsFromAuthor(page, author);
		return Page();
	}
}
