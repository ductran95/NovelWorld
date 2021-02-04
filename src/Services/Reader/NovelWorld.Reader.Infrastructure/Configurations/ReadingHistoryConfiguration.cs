using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.Reader.Data.Entities;

namespace NovelWorld.Reader.Infrastructure.Configurations
{
    public class ReadingHistoryConfiguration: EfCoreEntityConfiguration<ReadingHistory>
    {
        public override void Configure(EntityTypeBuilder<ReadingHistory> builder)
        {
            base.Configure(builder);
            builder.ToTable("ReadingHistories", schema: "reader");
        }
    }
}