using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class DeskUserConfiguration :IEntityTypeConfiguration<DeskUser>
	{
		public void Configure(EntityTypeBuilder<DeskUser> builder)
		{
			builder.ToTable("DeskUser");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.UserId).IsRequired();
			builder.Property(e => e.DeskId).IsRequired();
			builder.Property(e => e.UserDeskRoleId).IsRequired();
			builder.Property(e => e.UserDeskTypeId).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.Desk)
				.WithMany()
				.HasForeignKey(e => e.DeskId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.UserDeskRole)
				.WithMany()
				.HasForeignKey(e => e.UserDeskRoleId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.UserDeskType)
				.WithMany()
				.HasForeignKey(e => e.UserDeskTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => new { e.DeskId, e.UserId }).IsUnique();

			builder.HasIndex(e => e.UserId);
			builder.HasIndex(e => e.DeskId);
			builder.HasIndex(e => e.UserDeskRoleId);
			builder.HasIndex(e => e.UserDeskTypeId);
		}
	}
}
