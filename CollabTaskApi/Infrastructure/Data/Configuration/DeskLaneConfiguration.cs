using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class DeskLaneConfiguration : IEntityTypeConfiguration<DeskLane>
	{
		public void Configure(EntityTypeBuilder<DeskLane> builder)
		{
			builder.ToTable("DeskLane");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.DeskId).IsRequired();
			builder.Property(e => e.LaneId).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.Desk)
				.WithMany()
				.HasForeignKey(e => e.DeskId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.Lane)
				.WithMany()
				.HasForeignKey(e => e.LaneId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.DeskId);
			builder.HasIndex(e => e.LaneId);
		}
	}
}
