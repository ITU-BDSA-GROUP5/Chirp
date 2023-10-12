using System.Data.Common;
using SQLitePCL;

public class ChirpDBContextTests
{
	[Fact]
	public void Cheep_PropertyTest()
	{
		int cheepId = 1;
		string text = "test";
		var date = DateTime.Now;
		int authorId = 1;

		var author = new Author
		{
			Name = "name",
			Email = "email"
		};

		var cheep = new Cheep
		{
			AuthorId = authorId,
			Author = author,
			CheepId = cheepId,
			Text = text,
			TimeStamp = date
		};

		Assert.Equal(author, cheep.Author);
		Assert.Equal(authorId, cheep.AuthorId);
		Assert.Equal(cheepId, cheep.CheepId);
		Assert.Equal(text, cheep.Text);
		Assert.Equal(date, cheep.TimeStamp);
	}

	[Fact]
	public void Author_PropertyTest()
	{
		int authorId = 1;
		string email = "lol@lol.dk";
		string name = "name";
		var cheepList = new List<Cheep>();

		var author = new Author
		{
			AuthorId = authorId,
			Email = email,
			Name = name,
			Cheeps = cheepList
		};



		Assert.Equal(authorId, author.AuthorId);
		Assert.Equal(email, author.Email);
		Assert.Equal(name, author.Name);
		Assert.Equal(cheepList, author.Cheeps);
	}
}