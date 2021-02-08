using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.MasterData.Infrastructure.Configurations
{
    public class CategoryConfiguration: EfCoreEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.ToTable("Categories", schema: "masterdata");
            builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextFieldMaxLength);
        }
    }
}