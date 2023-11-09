using System.Security.Claims;
using Chirp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages
{
    public class SendCheepModel : PageModel
    {

        [BindProperty]
        public string? CheepMessage { get; set; }

        private readonly IAuthorRepository AuthorRepository;
        private readonly ICheepRepository CheepRepository;

        public SendCheepModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
        {
            AuthorRepository = authorRepository;
            CheepRepository = cheepRepository;
        }

        public IActionResult OnPost()
        {
            string email = User.Claims.Where(a => a.Type == "emails").Select(e => e.Value).Single();

            Console.WriteLine(email + " wrote: " + CheepMessage);

            // check if author exists
            // fetch author
            // possibly create author

            AuthorDTO? Author = AuthorRepository.GetAuthorByEmail(email).First();

            if (Author == null)
            {
                Author = CreateNewAuthor();
            }

            CreateCheepDTO cheep = new CreateCheepDTO()
            {
                CheepGuid = new Guid(),
                Text = CheepMessage,
                Name = Author.Name,
                Email = Author.Email
            };

            return Redirect("/");
        }

        private AuthorDTO CreateNewAuthor()
        {
            throw new NotImplementedException();
        }
    }
}
