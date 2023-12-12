using Chirp.Core;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
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
		public string? ErrorMessage { get; set; }
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
			return Redirect("/MicrosoftIdentity/Account/SignOut");
		}

		public async Task<ActionResult> OnPostDownload()
		{
			string? name = User.Identity?.Name;
			if (name != null)
			{
				List<CheepDTO> cheeps = CheepRepository.GetCheepsFromAuthor(name);
				List<string> followeeNames = AuthorRepository.GetFollowing(name).Select(a => a.Name).ToList();

				string token = User.FindFirst("idp_access_token")?.Value
					?? throw new Exception("Github token not found");
				string mail = await GithubHelper.GetUserEmailGithub(token, name);

				MyDataRecord myData = new MyDataRecord(name, mail, cheeps, followeeNames);
				string myDataJson = JsonSerializer.Serialize(myData);

				return File(System.Text.Encoding.UTF8.GetBytes(myDataJson), "text/json", "My_Chirp_Data.json");
			}
			else
			{
				ErrorMessage = "Name is null!";
				return await OnGet();
			}
		}

		public IActionResult OnPostUnfollow(string followeeName, string followerName)
		{
			AuthorRepository.UnfollowAuthor(followerName ?? throw new Exception("Name is null!"), followeeName);
			return Redirect(PageUrl ?? "/");
		}

		public IActionResult OnPostToggleMusic()
		{
			if (Request.Cookies["Music"] == "enabled")
			{
				Response.Cookies.Delete("Music");
			}
			else
			{
				Response.Cookies.Append("Music", "enabled");
			}

			return Redirect(PageUrl ?? "/");
		}
	}
}
