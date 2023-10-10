using System.Data.Common;
using SQLitePCL;

public class ChirpDBContextTests
{
	[Fact]
	public void test()
	{
		var dBContext = new ChirpDBContext();
		var author = new Author();
		var cheep = new Cheep();

		int authorId = 1;
		string email = "lol@lol.dk";
		string name = "name";
		var cheepList = new List<Cheep>();

		int cheepId = 1;
		string text = "test";
		var date = DateTime.Now;

		author.AuthorId = authorId;
		author.Email = email;
		author.Name = name;
		author.Cheeps = cheepList;

		cheep.AuthorId = authorId;
		cheep.Author = author;
		cheep.CheepId = cheepId;
		cheep.Text = text;
		cheep.TimeStamp = date;

		string path = dBContext.DbPath;

		Assert.Equal(authorId, author.AuthorId);
		Assert.Equal(email, author.Email);
		Assert.Equal(name, author.Name);
		Assert.Equal(cheepList, author.Cheeps);
		Assert.Equal(cheepId, cheep.CheepId);
		Assert.Equal(text, cheep.Text);
		Assert.Equal(date, cheep.TimeStamp);
		Assert.EndsWith("/data/Chirp.db", path);
	}
}