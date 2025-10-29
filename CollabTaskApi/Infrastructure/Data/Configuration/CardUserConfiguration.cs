using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class CardUserConfiguration :IEntityTypeConfiguration<CardUser>
	{
		public void Configure(EntityTypeBuilder<CardUser> builder)
		{
			builder.ToTable("CardUser");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.CardId).IsRequired();
			builder.Property(e => e.UserId).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.Card)
				.WithMany()
				.HasForeignKey(e => e.CardId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => new { e.CardId, e.UserId }).IsUnique();

			builder.HasIndex(e => e.CardId);
			builder.HasIndex(e => e.UserId);
		}
	}
}
