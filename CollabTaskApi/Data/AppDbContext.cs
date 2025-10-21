using CollabTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users => Set<User>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			base.OnModelCreating(modelBuilder);
		}
	}
}
