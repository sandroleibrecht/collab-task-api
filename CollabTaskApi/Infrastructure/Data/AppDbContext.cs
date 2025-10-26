using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Infrastructure.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Desk> Desks => Set<Desk>();
		public DbSet<UserImage> UserImages => Set<UserImage>();
		public DbSet<DeskInvitation> DeskInvitations => Set<DeskInvitation>();
		public DbSet<UserDesk> UserDesks => Set<UserDesk>();
		public DbSet<UserDeskRole> UserDeskRoles => Set<UserDeskRole>();
		public DbSet<UserDeskType> UserDeskTypes => Set<UserDeskType>();
		public DbSet<UserRefreshToken> UserRefreshToken => Set<UserRefreshToken>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<Desk>().ToTable("Desk");
			modelBuilder.Entity<UserImage>().ToTable("UserImage");
			modelBuilder.Entity<DeskInvitation>().ToTable("DeskInvitation");
			modelBuilder.Entity<UserDesk>().ToTable("UserDesk");
			modelBuilder.Entity<UserDeskRole>().ToTable("UserDeskRole");
			modelBuilder.Entity<UserDeskType>().ToTable("UserDeskType");
			modelBuilder.Entity<UserRefreshToken>().ToTable("UserRefreshToken");
			base.OnModelCreating(modelBuilder);
		}
	}
}
