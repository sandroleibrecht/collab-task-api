using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class DeskConfiguration :IEntityTypeConfiguration<Desk>
	{
		public void Configure(EntityTypeBuilder<Desk> builder)
		{
			builder.ToTable("Desk");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.Name).IsRequired();
			builder.Property(e => e.Color).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();
		}
	}
}
