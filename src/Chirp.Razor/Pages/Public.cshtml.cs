using Chirp.Razor.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
	private readonly ICheepRepository _repository;
	public required List<CheepDTO> Cheeps { get; set; }

	public PublicModel(ICheepRepository repository/*, IAuthorRepository authorRepository*/)
	{
		_repository = repository;
	}


	public ActionResult OnGet([FromQuery(Name = "page")] int page = 1)
	{
		Cheeps = _repository.GetCheeps(page);

		//_repository.createNewCheep(new Guid(), );

		//List<Author> authorList = _authorRepository.getAuthorByEmail("jeremymicheal@gmail.com");
		//Console.WriteLine(authorList.Count);
		//_authorRepository.createNewAuthor(new Guid(), "Jeremy Micheal", "jeremymicheal@gmail.com");
		//      authorList = _authorRepository.getAuthorByEmail("jeremymicheal@gmail.com");
		//      Console.WriteLine(authorList.Count);

		//List<Author> authorList = _authorRepository.getAuthorByEmail("Mellie+Yost@ku.dk");
		//Console.WriteLine(authorList.Count);
		//foreach (var item in authorList) { Console.WriteLine(item.Name + ", " + item.Email + ", " + item.AuthorId); }
		//Console.WriteLine("--- " + _authorRepository.getAuthorByName("Roger Hilstand"));
		return Page();
	}
}
