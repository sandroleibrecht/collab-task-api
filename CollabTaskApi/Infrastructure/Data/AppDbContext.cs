using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Infrastructure.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Card> Cards => Set<Card>();
		public DbSet<CardUser> CardUsers => Set<CardUser>();
		public DbSet<Desk> Desks => Set<Desk>();
		public DbSet<DeskInvitation> DeskInvitations => Set<DeskInvitation>();
		public DbSet<DeskLane> DeskLanes => Set<DeskLane>();
		public DbSet<DeskUser> DeskUsers => Set<DeskUser>();
		public DbSet<Lane> Lanes => Set<Lane>();
		public DbSet<LaneCard> LaneCards => Set<LaneCard>();
		public DbSet<User> Users => Set<User>();
		public DbSet<UserDeskRole> UserDeskRoles => Set<UserDeskRole>();
		public DbSet<UserDeskType> UserDeskTypes => Set<UserDeskType>();
		public DbSet<UserImage> UserImages => Set<UserImage>();
		public DbSet<UserRefreshToken> UserRefreshToken => Set<UserRefreshToken>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
