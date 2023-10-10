using System.Data.Common;
using SQLitePCL;

public class ChirpDBContextTests
{

	[Fact]
	public void testDBContext()
	{
		var dBContext = new ChirpDBContext();
		string path = dBContext.DbPath;
		Assert.EndsWith("/data/Chirp.db", path);
	}

	[Fact]
	public void testCheep()
	{
		var author = new Author();
		var cheep = new Cheep();

		int cheepId = 1;
		string text = "test";
		var date = DateTime.Now;
		int authorId = 1;

		cheep.AuthorId = authorId;
		cheep.Author = author;
		cheep.CheepId = cheepId;
		cheep.Text = text;
		cheep.TimeStamp = date;

		Assert.Equal(author, cheep.Author);
		Assert.Equal(authorId, cheep.AuthorId);
		Assert.Equal(cheepId, cheep.CheepId);
		Assert.Equal(text, cheep.Text);
		Assert.Equal(date, cheep.TimeStamp);
	}

	[Fact]
	public void testAuthor()
	{
		var author = new Author();

		int authorId = 1;
		string email = "lol@lol.dk";
		string name = "name";
		var cheepList = new List<Cheep>();

		author.AuthorId = authorId;
		author.Email = email;
		author.Name = name;
		author.Cheeps = cheepList;

		Assert.Equal(authorId, author.AuthorId);
		Assert.Equal(email, author.Email);
		Assert.Equal(name, author.Name);
		Assert.Equal(cheepList, author.Cheeps);
	}
}