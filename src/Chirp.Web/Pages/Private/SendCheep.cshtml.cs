using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages
{
    public class SendCheepModel : PageModel
    {

        [BindProperty]
        public required string Text { get; set; }

        public IActionResult OnGet()
        {
            Text = "onGet";
            System.Console.WriteLine(Text);
            return Page();
        }

        public IActionResult OnPost()
        {
            System.Console.WriteLine(Text);
            return RedirectToPage("/");
        }
    }
}
