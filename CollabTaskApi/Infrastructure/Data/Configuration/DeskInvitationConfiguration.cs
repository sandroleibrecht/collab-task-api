using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTaskApi.Infrastructure.Data.Configuration
{
	public class DeskInvitationConfiguration : IEntityTypeConfiguration<DeskInvitation>
	{
		public void Configure(EntityTypeBuilder<DeskInvitation> builder)
		{
			builder.ToTable("DeskInvitation");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.SenderUserId).IsRequired();
			builder.Property(e => e.ReceiverUserId).IsRequired();
			builder.Property(e => e.DeskId).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();

			builder.HasOne(e => e.SenderUser)
				.WithMany()
				.HasForeignKey(e => e.SenderUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ReceiverUser)
				.WithMany()
				.HasForeignKey(e => e.ReceiverUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.Desk)
				.WithMany()
				.HasForeignKey(e => e.DeskId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.SenderUserId);
			builder.HasIndex(e => e.ReceiverUserId);
			builder.HasIndex(e => e.DeskId);
		}
	}
}
