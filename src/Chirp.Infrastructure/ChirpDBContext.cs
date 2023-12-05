using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
	public DbSet<Author> Authors { get; set; }
	public DbSet<Cheep> Cheeps { get; set; }
	public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Author>()
			.HasMany(a => a.Followers)
			.WithMany(a => a.Following);
		modelBuilder.Entity<Cheep>().ToTable("Cheeps");

		modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
	}
}