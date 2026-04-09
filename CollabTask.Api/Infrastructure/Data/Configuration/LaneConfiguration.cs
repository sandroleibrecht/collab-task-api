using CollabTask.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollabTask.Api.Infrastructure.Data.Configuration
{
	public class LaneConfiguration : IEntityTypeConfiguration<Lane>
	{
		public void Configure(EntityTypeBuilder<Lane> builder)
		{
			builder.ToTable("Lane");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.Name);
			builder.Property(e => e.Order).IsRequired();
			builder.Property(e => e.CreatedAt).IsRequired();			
		}
	}
}
