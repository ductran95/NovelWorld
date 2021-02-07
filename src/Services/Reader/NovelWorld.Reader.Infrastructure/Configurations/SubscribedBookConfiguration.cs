using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.Reader.Data.Entities;

namespace NovelWorld.Reader.Infrastructure.Configurations
{
    public class SubscribedBookConfiguration: EfCoreEntityConfiguration<SubscribedBook>
    {
        public override void Configure(EntityTypeBuilder<SubscribedBook> builder)
        {
            base.Configure(builder);
            builder.ToTable("SubscribedBooks", schema: "reader");
        }
    }
}