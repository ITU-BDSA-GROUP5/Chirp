public class ChirpDBContextTests
{
	[Fact]
	public void Cheep_PropertyTest()
	{
		Guid cheepId = new Guid();
		string text = "test";
		var date = DateTime.Now;
		Guid authorId = new Guid();

		var author = new Author
		{
			AuthorId = authorId,
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
		Guid authorId = new Guid();
		string email = "email@domain.dk";
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