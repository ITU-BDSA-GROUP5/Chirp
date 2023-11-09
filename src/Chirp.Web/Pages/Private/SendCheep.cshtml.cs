using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages
{
    public class SendCheepModel : PageModel
    {

        [BindProperty]
        public string? CheepMessage { get; set; }

        public IActionResult OnPost()
        {
            Console.WriteLine("user wrote: " + CheepMessage);

            return Redirect("/");
        }
    }
}
