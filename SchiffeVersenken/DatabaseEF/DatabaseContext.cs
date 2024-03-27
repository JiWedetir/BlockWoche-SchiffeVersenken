using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SchiffeVersenken.DatabaseEF.Models;

public class DatabaseContext : DbContext
{
	public DbSet<UserEF> Users { get; set; }
	public DbSet<HighScoreEF> HighScores { get; set; }

	public string DbPath { get; }

	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "schiffeversenken.db");
	}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={DbPath}");
	}
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
	public DatabaseContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		var dbPath = Path.Join(path, "schiffeversenken.db");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");

		return new DatabaseContext(optionsBuilder.Options);
	}
}