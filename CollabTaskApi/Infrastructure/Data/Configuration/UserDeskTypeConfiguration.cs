using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class UserDeskTypeConfiguration : IEntityTypeConfiguration<UserDeskType>
	{
		public void Configure(EntityTypeBuilder<UserDeskType> builder)
		{
			builder.ToTable("UserDeskType");

			builder.HasKey(e => e.Id);
			
			builder.Property(e => e.Name).IsRequired();

			builder.HasIndex(e => e.Name).IsUnique();
		}
	}
}
