using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
	private readonly ICheepRepository _repository;
	public required List<CheepDTO> Cheeps { get; set; }

	public PublicModel(ICheepRepository repository)
	{
		_repository = repository;
	}


	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = _repository.GetCheeps(page);

		return Page();
	}
}
