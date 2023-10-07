using Microsoft.EntityFrameworkCore;
public class ChirpDBContext : DbContext
{
	public DbSet<Author> Authors { get; set; }
	public DbSet<Cheep> Cheeps { get; set; }
	public string DbPath { get; }
	public ChirpDBContext()
	{
		var path = Path.GetTempPath();
		DbPath = Path.Combine(path, "Chirp.db");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={DbPath}");
	}
}