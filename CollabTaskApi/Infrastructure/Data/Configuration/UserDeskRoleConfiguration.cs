using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class UserDeskRoleConfiguration : IEntityTypeConfiguration<UserDeskRole>
	{
		public void Configure(EntityTypeBuilder<UserDeskRole> builder)
		{
			builder.ToTable("UserDeskRole");

			builder.HasKey(e => e.Id);
			
			builder.Property(e => e.Name).IsRequired();

			builder.HasIndex(e => e.Name).IsUnique();
		}
	}
}
