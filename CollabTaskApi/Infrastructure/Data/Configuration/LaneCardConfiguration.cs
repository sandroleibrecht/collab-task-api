using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class LaneCardConfiguration : IEntityTypeConfiguration<LaneCard>
	{
		public void Configure(EntityTypeBuilder<LaneCard> builder)
		{
			builder.ToTable("LaneCard");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.LaneId).IsRequired();
			builder.Property(e => e.CardId).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.Lane)
				.WithMany()
				.HasForeignKey(e => e.LaneId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.Card)
				.WithMany()
				.HasForeignKey(e => e.CardId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.LaneId);
			builder.HasIndex(e => e.CardId);
		}
	}
}
