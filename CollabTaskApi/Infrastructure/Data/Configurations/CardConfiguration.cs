using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configurations
{
	public class CardConfiguration :IEntityTypeConfiguration<Card>
	{
		public void Configure(EntityTypeBuilder<Card> builder)
		{
			builder.ToTable("Card");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.Name).IsRequired();
			builder.Property(e => e.Description);
			builder.Property(e => e.Order).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();
		}
	}
}
