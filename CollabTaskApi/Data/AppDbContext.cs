using CollabTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users => Set<User>();
		public DbSet<UserDeskRole> UserDeskRoles => Set<UserDeskRole>();
		public DbSet<UserDeskType> UserDeskTypes => Set<UserDeskType>();
		public DbSet<Desk> Desks => Set<Desk>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<UserDeskRole>().ToTable("UserDeskRole");
			modelBuilder.Entity<UserDeskType>().ToTable("UserDeskType");
			modelBuilder.Entity<Desk>().ToTable("Desk");
			base.OnModelCreating(modelBuilder);
		}
	}
}
