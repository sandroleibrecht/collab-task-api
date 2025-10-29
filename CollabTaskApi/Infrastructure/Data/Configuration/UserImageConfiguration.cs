using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
	{
		public void Configure(EntityTypeBuilder<UserImage> builder)
		{
			builder.ToTable("UserImage");

			builder.HasKey(e => e.Id);
			
			builder.Property(e => e.UserId).IsRequired();
			builder.Property(e => e.FilePath).IsRequired();

			builder.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.UserId).IsUnique();
		}
	}
}
