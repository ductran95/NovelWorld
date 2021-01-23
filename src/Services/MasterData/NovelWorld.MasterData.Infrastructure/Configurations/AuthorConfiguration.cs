using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.MasterData.Data.Entities;

namespace NovelWorld.MasterData.Infrastructure.Configurations
{
    public class AuthorConfiguration: EfCoreEntityConfiguration<Author>
    {
        public override void Configure(EntityTypeBuilder<Author> builder)
        {
            base.Configure(builder);
            builder.ToTable("Authors", schema: "masterdata");
            builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(CommonValidationRules.TextFieldMaxLength);
            builder.Property(x => x.Description).IsUnicode().HasMaxLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}