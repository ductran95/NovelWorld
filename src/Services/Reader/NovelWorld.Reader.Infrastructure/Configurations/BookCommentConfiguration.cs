using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelWorld.Data.Constants;
using NovelWorld.Infrastructure.EntityFrameworkCore.Configurations;
using NovelWorld.Reader.Data.Entities;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.Reader.Infrastructure.Configurations
{
    public class BookCommentConfiguration: EfCoreEntityConfiguration<BookComment>
    {
        public override void Configure(EntityTypeBuilder<BookComment> builder)
        {
            base.Configure(builder);
            builder.ToTable("BookComments", schema: "reader");
            builder.Property(x => x.Content).IsRequired().IsUnicode().HasMaxLength(SharedValidationRules.TextAreaMaxLength);
        }
    }
}