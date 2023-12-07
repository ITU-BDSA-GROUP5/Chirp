using Chirp.Core;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
	public class UserModel : PageModel
	{
		private readonly IAuthorRepository AuthorRepository;
		private readonly ICheepRepository CheepRepository;
		public required List<CheepDTO> Cheeps { get; set; }
		public required List<AuthorDTO> Followees { get; set; }

		public int PageNumber { get; set; }
		public int LastPageNumber { get; set; }
		public string? PageUrl { get; set; }

		public string? Mail { get; set; }


		public UserModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
		{
			AuthorRepository = authorRepository;
			CheepRepository = cheepRepository;
		}

		public async Task<ActionResult> OnGet([FromQuery(Name = "page")] int page = 1)
		{
			string? name = User.Identity?.Name;

			if (name != null)
			{
				Cheeps = CheepRepository.GetCheepsFromAuthor(page, name);
				PageNumber = page;
				LastPageNumber = CheepRepository.GetPageAmount(name);
				Followees = AuthorRepository.GetFollowing(name);
				PageUrl = HttpContext.Request.GetEncodedUrl().Split("?")[0];

				string token = User.FindFirst("idp_access_token")?.Value
					?? throw new Exception("Github token not found");

				Mail = await GithubHelper.GetUserEmailGithub(token, name);
			}

			return Page();
		}

		//This deletes the user and associated cheeps from the database. Note that it does not log out or change anything azure and cookie related
		public ActionResult OnPostDelete()
		{
			AuthorRepository.DeleteAuthorByName(User.Identity?.Name!);
			return Redirect("/");
		}

		public ActionResult OnPostDownload()
		{
			string? name = User.Identity?.Name;

			if (name != null)
			{
				Cheeps = CheepRepository.GetCheepsFromAuthor(name);
				Followees = AuthorRepository.GetFollowing(name);

				MyDataRecord myData = new MyDataRecord(Cheeps, Followees);

				string json = JsonSerializer.Serialize(myData);

				return File(System.Text.Encoding.UTF8.GetBytes(json), "text/json", "My_Chirp_Data.json");
			}
			else
			{
				return Redirect("/");
			}
		}

		public async Task<IActionResult> OnPostUnfollow(string followeeName, string followerName)
		{
			await AuthorRepository.UnfollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
			return Redirect(PageUrl ?? "/");
		}
	}
}
