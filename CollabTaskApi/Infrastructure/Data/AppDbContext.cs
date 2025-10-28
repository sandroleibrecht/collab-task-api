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
		public DbSet<DeskUser> DeskUsers => Set<DeskUser>();
		public DbSet<UserDeskRole> UserDeskRoles => Set<UserDeskRole>();
		public DbSet<UserDeskType> UserDeskTypes => Set<UserDeskType>();
		public DbSet<UserRefreshToken> UserRefreshToken => Set<UserRefreshToken>();
		public DbSet<Card> Cards => Set<Card>();
		public DbSet<CardUser> CardUsers => Set<CardUser>();
		public DbSet<DeskLane> DeskLanes => Set<DeskLane>();
		public DbSet<Lane> Lanes => Set<Lane>();
		public DbSet<LaneCard> LaneCards => Set<LaneCard>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<Desk>().ToTable("Desk");
			modelBuilder.Entity<UserImage>().ToTable("UserImage");
			modelBuilder.Entity<DeskInvitation>().ToTable("DeskInvitation");
			modelBuilder.Entity<DeskUser>().ToTable("DeskUser");
			modelBuilder.Entity<UserDeskRole>().ToTable("UserDeskRole");
			modelBuilder.Entity<UserDeskType>().ToTable("UserDeskType");
			modelBuilder.Entity<UserRefreshToken>().ToTable("UserRefreshToken");
			modelBuilder.Entity<Card>().ToTable("Card");
			modelBuilder.Entity<CardUser>().ToTable("CardUser");
			modelBuilder.Entity<DeskLane>().ToTable("DeskLane");
			modelBuilder.Entity<Lane>().ToTable("Lane");
			modelBuilder.Entity<LaneCard>().ToTable("LaneCard");
			base.OnModelCreating(modelBuilder);
		}
	}
}
