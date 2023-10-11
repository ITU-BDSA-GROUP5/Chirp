using Microsoft.EntityFrameworkCore;
public class ChirpDBContext : DbContext
{
	public DbSet<Author> Authors { get; set; }
	public DbSet<Cheep> Cheeps { get; set; }
	public string DbPath { get; }
	public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
	{
		var fullPath = Path.GetFullPath("./data/");
		DbPath = Path.Combine(fullPath, "Chirp.db");
	}

	public ChirpDBContext()
	{
		var fullPath = Path.GetFullPath("./data/");
		DbPath = Path.Combine(fullPath, "Chirp.db");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={DbPath}");
	}
}