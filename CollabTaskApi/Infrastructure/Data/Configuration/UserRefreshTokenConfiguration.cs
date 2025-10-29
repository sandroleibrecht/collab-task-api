using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
	{
		public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
		{
			builder.ToTable("UserRefreshToken");

			builder.HasKey(e => e.Id);
			
			builder.Property(e => e.UserId).IsRequired();
			builder.Property(e => e.Token).IsRequired();
			builder.Property(e => e.ExpiresAt).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.UserId);
		}
	}
}
