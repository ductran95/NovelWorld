using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.MasterData.Infrastructure.Configurations
{
    public class ChapterConfiguration: EfCoreEntityConfiguration<Chapter>
    {
        public override void Configure(EntityTypeBuilder<Chapter> builder)
        {
            base.Configure(builder);
            builder.ToTable("Chapters", schema: "masterdata");
            builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextFieldMaxLength);
            builder.Property(x => x.Content).IsRequired().IsUnicode();
        }
    }
}