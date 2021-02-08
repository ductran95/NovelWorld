using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.MasterData.Infrastructure.Configurations
{
    public class BookConfiguration: EfCoreEntityConfiguration<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);
            builder.ToTable("Books", schema: "masterdata");
            builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextFieldMaxLength);
            builder.Property(x => x.Description).IsUnicode().HasMaxLength(SharedValidationRules.TextAreaMaxLength);
            builder.Property(x => x.Cover).IsRequired().IsUnicode();
        }
    }
}