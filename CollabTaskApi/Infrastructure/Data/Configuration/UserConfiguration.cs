using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class UserConfiguration :IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("User");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.Name).IsRequired();
			builder.Property(e => e.Email).IsRequired();
			builder.Property(e => e.Password).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasIndex(e => e.Email).IsUnique();
		}
	}
}
