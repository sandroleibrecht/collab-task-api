using CollabTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users => Set<User>();
		public DbSet<Workspace> Workspaces => Set<Workspace>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<Workspace>().ToTable("Workspace");

			base.OnModelCreating(modelBuilder);
		}
	}
}
