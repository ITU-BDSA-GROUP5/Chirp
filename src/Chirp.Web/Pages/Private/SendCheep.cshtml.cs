using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages
{
	public class SendCheepModel : PageModel
	{

		[BindProperty]
		public string? CheepMessage { get; set; }

		public IActionResult OnGet()
		{
			CheepMessage = "Brevity is the soul of the wit.";
			Console.WriteLine(CheepMessage);
			return Page();
		}

		public IActionResult OnPost()
		{
			Console.WriteLine(CheepMessage);
			return Redirect("https://localhost:7102/");
		}
	}
}
